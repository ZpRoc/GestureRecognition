# -*- coding: utf-8 -*-

''' rdata_handle.py

    Define some functions about reading data. 

    Important：
        get_img_batches: Get image tfrecord batches
        get_txt_batches: Get   txt tfrecord batches

    Useful: 
        get_num_example: Get the number of examples in .tfrecord files. 

    Notes:
        tf.train.batch & tf.train.shuffle_batch:
            原文：https://blog.csdn.net/ying86615791/article/details/73864381
            capacity: 队列的长度
            min_after_dequeue: 出队后，队列至少剩下 min_after_dequeue 个数据
            假设现在有个 test.tfrecord 文件，里面按从小到大顺序存放整数 0~100: 
            tf.train.batch
                按顺序读取数据，队列中的数据始终是一个有序的队列
                比如队列的 capacity=20，开始队列内容为 0,1,...,19 --> 读取10条记录后，队列剩下 10,11,...,19 --> 然后又补充10条变成 10,11,...,29,
                队头一直按顺序补充，队尾一直按顺序出队，到了第100条记录后，又重头开始补充 0,1,2...
            tf.train.shuffle_batch
                将队列中数据打乱后，再读取出来，因此队列中剩下的数据也是乱序的，队头也是一直在补充（我猜也是按顺序补充）
                比如 batch_size=5, capacity=10, min_after_dequeue=5,
                初始是有序的 0,1，..,9(10条记录)，
                然后打乱 8,2,6,4,3,7,9,2,0,1(10条记录),
                队尾取出 5 条，剩下 7,9,2,0,1(5条记录),
                然后又按顺序补充进来，变成 7,9,2,0,1,10,11,12,13,14(10条记录)，
                再打乱 13,10,2,7,0,12...1(10条记录)，再出队...
'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import os
import logging

import numpy as np
import tensorflow as tf
import PIL.Image as ImgProc
import matplotlib.pyplot as plt

import img_handle


# ---------------------------------------------------------------------------------------------------- #
# ------------------------------------- Get tfrecord attributes -------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def get_num_example(data_url, file_prefix):
    ''' Get the number of examples in .tfrecord files. 

        Args: 
            data_url   : The url of .tfrecord files.
            file_prefix: The prefix of TFRecord file names, one of ['train', 'vld', 'test'].

        Return:
            num_example: The number of examples. 

        Raises:
            raise ValueError('Cannot find the .tfrecord files. ')
    '''
    ### Get all .tfrecord files
    tfr_files = tf.gfile.Glob(os.path.join(data_url, file_prefix+'*.tfrecord'))
    if len(tfr_files) == 0:
        raise ValueError('Cannot find the .tfrecord files. ')

    ### Count the number of example
    num_example = 0
    for tfr_file in tfr_files:
        for record in tf.python_io.tf_record_iterator(tfr_file):
            num_example += 1

    ### Return
    return num_example


# ---------------------------------------------------------------------------------------------------- #
# ------------------------------------ Get image tfrecord batches ------------------------------------ #
# ---------------------------------------------------------------------------------------------------- #
def get_img_batches(data_url, file_prefix, batch_size, image_infos, adjust_params=None, random_params=None, 
                    is_training=True, num_threads=4, capacity=1024, min_after_dequeue=512):
    ''' Get image tfrecord batches. 

        Args: 
            data_url         : The url of .tfrecord files.
            file_prefix      : The prefix of TFRecord file names, one of ['train', 'vld', 'test'].
            batch_size       : Training parameter, batch size. 
            image_infos      : The infos of image data.
            adjust_params    : The adjust parameters of image pre-processing. 
            random_params    : The random parameters of image pre-processing. 
            is_training      : If Ture, use tf.train.shuffle_batch for training, else, use tf.train.batch for testing. 
            num_threads      : 
            capacity         : 
            min_after_dequeue: 

        Return: 
            data_dicts: {filename:filename, data_raw:data_raw, labelname:labelname, labelval:labelval}

        Raises:
            ValueError('Cannot find the .tfrecord files. ')

    '''
    ### Get all .tfrecord files
    tfr_files = tf.gfile.Glob(os.path.join(data_url, file_prefix+'*.tfrecord'))
    if len(tfr_files) == 0:
        raise ValueError('Cannot find the .tfrecord files. ')

    ### Define the filename_queue and serialized_example
    filename_queue        = tf.train.string_input_producer(tfr_files, capacity=capacity)
    reader                = tf.TFRecordReader()
    _, serialized_example = reader.read(filename_queue)

    ### Decode the example
        # filename : the url of image
        # data_raw : the image stored as btyes
        # labelname: the label name
        # labelval : the label value
    features = tf.parse_single_example(serialized_example, 
                    features = {'filename' :tf.FixedLenFeature([], tf.string), 
                                'data_raw' :tf.FixedLenFeature([], tf.string),
                                'labelname':tf.FixedLenFeature([], tf.string),
                                'labelval' :tf.FixedLenFeature([], tf.int64),
                               })

    ### Get the detail datas
    filename  = tf.cast(features['filename']      , tf.string)
    data_raw  = tf.decode_raw(features['data_raw'], tf.uint8)
    labelname = tf.cast(features['labelname']     , tf.string)
    labelval  = tf.cast(features['labelval']      , tf.int32)

    ### Reshape data, if not, a undefined shape of tensor will be thrown. 
    data_raw = tf.reshape(data_raw, image_infos[0:3])

    ### Image pre-processing
    data_raw = img_handle.img_pproc(data_raw, adjust_params, random_params)

    ### Get data dict
    data_dict = {'filename' :filename, 
                 'data_raw' :data_raw, 
                 'labelname':labelname, 
                 'labelval' :labelval, 
                }

    ### Get the batches
    if is_training:     # For training
        data_dicts = tf.train.shuffle_batch(data_dict, batch_size, capacity, min_after_dequeue, num_threads=num_threads)
    
    else:               # For test
        data_dicts = tf.train.batch(data_dict, batch_size)

    ### Return
    return data_dicts


# ---------------------------------------------------------------------------------------------------- #
# ------------------------------------ Get txt tfrecord batches ------------------------------------ #
# ---------------------------------------------------------------------------------------------------- #
def get_txt_batches(data_url, file_prefix, batch_size, txt_infos, 
                    is_training=True, num_threads=4, capacity=1024, min_after_dequeue=512):
    ''' Get txt tfrecord batches. 

        Args: 
            data_url         : The url of .tfrecord files.
            file_prefix      : The prefix of TFRecord file names, one of ['train', 'vld', 'test'].
            batch_size       : Training parameter, batch size. 
            txt_infos        : The infos of txt data.
            is_training      : If Ture, use tf.train.shuffle_batch for training, else, use tf.train.batch for testing. 
            num_threads      : 
            capacity         : 
            min_after_dequeue: 

        Return: 
            data_dicts: {filename:filename, data_raw:data_raw, labelname:labelname, labelval:labelval}

        Raises:
            ValueError('Cannot find the .tfrecord files. ')

    '''
    ### Get all .tfrecord files
    tfr_files = tf.gfile.Glob(os.path.join(data_url, file_prefix+'*.tfrecord'))
    if len(tfr_files) == 0:
        raise ValueError('Cannot find the .tfrecord files. ')

    ### Define the filename_queue and serialized_example
    filename_queue        = tf.train.string_input_producer(tfr_files, capacity=capacity)
    reader                = tf.TFRecordReader()
    _, serialized_example = reader.read(filename_queue)

    ### Decode the example
        # filename : the url of txt file
        # data_raw : the txt data
        # labelname: the label name
        # labelval : the label value
    features = tf.parse_single_example(serialized_example, 
                    features = {'filename' :tf.FixedLenFeature([], tf.string), 
                                'data_raw' :tf.FixedLenFeature(txt_infos[0:1], tf.float32),
                                'labelname':tf.FixedLenFeature([], tf.string),
                                'labelval' :tf.FixedLenFeature([], tf.int64),
                               })

    ### Get the detail datas
    filename  = tf.cast(features['filename'] , tf.string)
    data_raw  = tf.cast(features['data_raw'] , tf.float32)
    labelname = tf.cast(features['labelname'], tf.string)
    labelval  = tf.cast(features['labelval'] , tf.int32)

    ### Get data dict
    data_dict = {'filename' :filename, 
                 'data_raw' :data_raw, 
                 'labelname':labelname, 
                 'labelval' :labelval, 
                }

    ### Get the batches
    if is_training:     # For training
        data_dicts = tf.train.shuffle_batch(data_dict, batch_size, capacity, min_after_dequeue, num_threads=num_threads)
    
    else:               # For test
        data_dicts = tf.train.batch(data_dict, batch_size)

    ### Return
    return data_dicts


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    pass
