# -*- coding: utf-8 -*-

''' wdata_handle.py

    Define some functions about writing data. 

    Important:
        write_img_tfrecords: Write image data sets and save as .tfrecord file
        write_txt_tfrecords: Write   txt data sets and save as .tfrecord file

    Useful:
        seg_data     : Segment the data sets into training sets, validation_sets and test sets. 
        get_data_info: Get the number and filenames of each label of given data sets. 

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import os
import sys
import logging
import random
import datetime

import numpy as np
import tensorflow as tf
import PIL.Image as ImgProc

import img_handle
import file_handle


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Useful functions ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def seg_data(url, unique_labels, file_exts, data_pct, seed=None):
    ''' Segment the data sets into training sets, validation_sets and test sets. 

        Args:
            url          : The url of given root directory. 
            unique_labels: The unique label names.             
            file_exts    : The extension of file, like ['bmp', 'jpg', 'png', 'tif', ...]
            data_pct     : The percentage of data sets, like [train_pct, vld_pct] = [80, 0]
            seed         : The random seed in shuffled_index

        Return:
            data_sets : The data sets. 
            train_sets: The training sets whose shape likes [filenames, labelnames, labelvals]
            vld_sets  : The validation sets whose shape likes [filenames, labelnames, labelvals]
            test_sets : The test sets whose shape likes [filenames, labelnames, labelvals]

        Raises:

    '''
    ### Get all files with given seed (make sure the result of shuffled_index is the same). 
    data_sets = file_handle.get_files(url, unique_labels, file_exts, seed=seed)

    ### Segment randomly according to the percentage
    # data set shape: filenames, labelnames, labelvals
    train_sets = [[], [], []]
    vld_sets   = [[], [], []]
    test_sets  = [[], [], []]
    data_num   = len(data_sets[0])
    if True:        # The data have been shuffled in function file_handle.get_files
        for i in range(data_num):
            if i < data_num * data_pct[0] / 100.0:
                train_sets[0].append(data_sets[0][i])
                train_sets[1].append(data_sets[1][i])
                train_sets[2].append(data_sets[2][i])
            elif i < data_num * (data_pct[0] + data_pct[1]) / 100.0:
                vld_sets[0].append(data_sets[0][i])
                vld_sets[1].append(data_sets[1][i])
                vld_sets[2].append(data_sets[2][i])
            else:
                test_sets[0].append(data_sets[0][i])
                test_sets[1].append(data_sets[1][i])
                test_sets[2].append(data_sets[2][i])
    else:           # The data have not been shuffled
        for i in range(data_num):
            chance = np.random.randint(100)
            if chance < data_pct[0]:
                train_sets[0].append(data_sets[0][i])
                train_sets[1].append(data_sets[1][i])
                train_sets[2].append(data_sets[2][i])
            elif chance < (data_pct[0] + data_pct[1]):
                vld_sets[0].append(data_sets[0][i])
                vld_sets[1].append(data_sets[1][i])
                vld_sets[2].append(data_sets[2][i])
            else:
                test_sets[0].append(data_sets[0][i])
                test_sets[1].append(data_sets[1][i])
                test_sets[2].append(data_sets[2][i])

    ### return
    return data_sets, train_sets, vld_sets, test_sets
    

def get_data_info(data_sets, unique_labels):
    ''' Get the number and filenames of each label of given data sets. 

        Args:
            data_sets    : The data sets whose shape likes [filenames, labelnames, labelvals]
            unique_labels: The unique label names. 

        Return:
            data_info: [nums, filenames] 

        Raises:

    '''
    # -------------------- Numbers -------------------- #
    ### Get the numbers of each label
    nums = []
    for label in unique_labels:
        nums.append(data_sets[1].count(label))

    ### Get the numbers string of each label
    nums_str = 'Total: {0:5d}'.format(sum(nums))
    for i in range(len(unique_labels)):
        # Add each label info string
        nums_str = '{0},  {1}: {2:5d}'.format(nums_str, unique_labels[i], nums[i])

    # -------------------- Filenames -------------------- #
    ### Get the filenames string of each label
    filenames_str = []
    for label in unique_labels:
        filename_str = ['\t{0} Files: '.format(label)]
        for index in [index for index, value in enumerate(data_sets[1]) if value==label]:
            filename_str.append('\t\t{0}'.format(data_sets[0][index]))
        filenames_str.append(filename_str)

    ### Return
    return [nums_str, filenames_str]


# ---------------------------------------------------------------------------------------------------- #
# -------------------------------------- Write .tfrecord files --------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def write_img_tfrecords(img_url, unique_labels, data_url, img_infos, file_exts=['bmp', 'jpg', 'png', 'tif'], data_pct=[80, 0], 
                       file_prefixs=['train', 'vld', 'test'], num_shards=[1, 0, 1], seed=None):
    ''' Write image data sets and save as .tfrecord file

        Args:
            img_url      : The url of image root directory. 
            unique_labels: The unique label names.                         
            data_url     : The url to save .tfrecord files. 
            img_infos    : The image infos [width, height, channel, mode]
            file_exts    : The extension of file, like ['bmp', 'jpg', 'png', 'tif', ...], default ['bmp', 'jpg', 'png', 'tif']
            data_pct     : Teh percentage of data sets, default [80, 0]
            file_prefixs : Use to define the filename of .tfrecords, like train/vld/test, default ['train', 'vld', 'test']
            num_shards   : The number of .tfrecords segmentation, default [1, 0, 1]
            seed         : The random seed in shuffled_index

        Return:

        Raise:
            raise ValueError('The data_url {0} does not exist. '.format(data_url))
    '''
    ### Raise
    if not os.path.exists(data_url):
        raise ValueError('The data_url {0} does not exist. '.format(data_url))

    ### Get all files under image_url, and segment them randomly using the given percentage. 
    data_sets, train_sets, vld_sets, test_sets = seg_data(img_url, unique_labels, file_exts, data_pct, seed)

    ### Write .tfrecord files. 
    for data_index, data_set in enumerate([train_sets, vld_sets, test_sets]):
        ### logging
        if num_shards[data_index] <= 0:
            logging.warning('Not creating {0} tfrecord files. '.format(file_prefixs[data_index]))
            continue
        if len(data_set[0]) < 2:
            logging.warning('The size of {0} data_set is too small. '.format(file_prefixs[data_index]))
            continue

        ### Segment the data sets using num_shards whose value is larger than 0
        ranges = []
        spacing = np.linspace(0, len(data_set[0]), num=num_shards[data_index]+1, dtype=np.int32)
        for i in range(len(spacing)-1):
            ranges.append([spacing[i], spacing[i+1]])

        ### Create .tfrecord file
        for start, end in ranges:
            # get the tfrecord filename
            tfrecord_filename = '{0}_{1:05d}_{2:05d}.tfrecord'.format(file_prefixs[data_index], start, end-1)
            # Write axamples
            print('{0}: Creating'.format(tfrecord_filename))
            writer = tf.python_io.TFRecordWriter(os.path.join(data_url, tfrecord_filename))
            for index in range(start, end):
                # Get the image and label
                filename  = data_set[0][index]
                img       = ImgProc.open(filename)
                labelname = data_set[1][index]
                labelval  = data_set[2][index]
                # Remove the root directory of image files
                fn_split = os.path.split(filename)
                filename = os.path.join(labelname, fn_split[1])
                # Get image infos
                img_cvt = ImgProc.Image.convert(img, mode=img_infos[3])
                img_rsz = ImgProc.Image.resize(img_cvt, img_infos[0:2])
                img_raw = ImgProc.Image.tobytes(img_rsz)
                # Add the image and label to a example
                    # filename : the url of image
                    # data_raw : the image stored as btyes
                    # labelname: the label name
                    # labelval : the label value
                example = tf.train.Example(
                                features = tf.train.Features(
                                    feature = {'filename' :tf.train.Feature(bytes_list = tf.train.BytesList(value = [str.encode(filename)])), 
                                               'data_raw' :tf.train.Feature(bytes_list = tf.train.BytesList(value = [img_raw])), 
                                               'labelname':tf.train.Feature(bytes_list = tf.train.BytesList(value = [str.encode(labelname)])), 
                                               'labelval' :tf.train.Feature(int64_list = tf.train.Int64List(value = [labelval])), 
                                            }))
                # Write to the .tfrecord file
                writer.write(example.SerializeToString())
            writer.close()
            print('{0}: Created'.format(tfrecord_filename))

    ### Write readme.txt
    with open(os.path.join(data_url, 'readme.txt'), 'w') as f:
        # -------------------- Created time -------------------- #
        f.write('Created at {0}\n\n'.format(datetime.datetime.now()))

        # -------------------- Tfrecord format -------------------- #
        f.write('Tfrecord format: \n')
        f.write('\tfilename  bytes  Filenames that have been removed root directory. \n')
        f.write('\tdata_raw  btyes  The image data that stores as btyes. \n')
        f.write('\tlabelname bytes  Label name. \n')
        f.write('\tlabelval  int    Label value that starts from 0. \n\n')

        # -------------------- Image url -------------------- #
        f.write('Image url: {0}\n\n'.format(img_url))

        # -------------------- Image infos -------------------- #
        f.write('Image infos [width, height, channel, mode]: {0}\n\n'.format(img_infos))

        # -------------------- Data infos -------------------- #
        ### Count the number of samples.
        data_infos  = get_data_info(data_sets , unique_labels)
        train_infos = get_data_info(train_sets, unique_labels)
        vld_infos   = get_data_info(vld_sets  , unique_labels)
        test_infos  = get_data_info(test_sets , unique_labels)

        ### Write data distribution
        f.write('Data distribution (seed={0}, data_pct={1}): \n'.format(seed, data_pct+[100-sum(data_pct)]))
        f.write('\t{0:8} {1}\n'.format('All', data_infos[0]))
        for i, infos in enumerate([train_infos, vld_infos, test_infos]):
            f.write('\t{0:8} {1}\n'.format(file_prefixs[i], infos[0]))
        f.write('\n')

        ### Write data filenames
        for i, infos in enumerate([train_infos, vld_infos, test_infos]):
            f.write('{0}: \n'.format(file_prefixs[i]))
            for info in infos[1]:
                for filename in info:
                    f.write('{0}\n'.format(filename))
            f.write('\n')
    

def write_txt_tfrecords(txt_url, unique_labels, data_url, txt_infos, file_exts=['txt'], data_pct=[80, 0], 
                       file_prefixs=['train', 'vld', 'test'], num_shards=[1, 0, 1], seed=None):
    ''' Write txt data sets and save as .tfrecord file

        Args:
            txt_url      : The url of txt root directory. 
            unique_labels: The unique label names.                         
            data_url     : The url to save .tfrecord files. 
            txt_infos    : The txt infos [x, y, z, type]
            file_exts    : The extension of file, like ['txt', ...], default ['txt']
            data_pct     : Teh percentage of data sets, default [80, 0]
            file_prefixs : Use to define the filename of .tfrecords, like train/vld/test, default ['train', 'vld', 'test']
            num_shards   : The number of .tfrecords segmentation, default [1, 0, 1]
            seed         : The random seed in shuffled_index

        Return:

        Raise:
            raise ValueError('The data_url {0} does not exist. '.format(data_url))
    '''
    ### Raise
    if not os.path.exists(data_url):
        raise ValueError('The data_url {0} does not exist. '.format(data_url))

    ### Get all files under txt_url, and segment them randomly using the given percentage. 
    data_sets, train_sets, vld_sets, test_sets = seg_data(txt_url, unique_labels, file_exts, data_pct, seed)

    ### Write .tfrecord files. 
    for data_index, data_set in enumerate([train_sets, vld_sets, test_sets]):
        ### logging
        if num_shards[data_index] <= 0:
            logging.warning('Not creating {0} tfrecord files. '.format(file_prefixs[data_index]))
            continue
        if len(data_set[0]) < 2:
            logging.warning('The size of {0} data_set is too small. '.format(file_prefixs[data_index]))
            continue

        ### Segment the data sets using num_shards whose value is larger than 0
        ranges = []
        spacing = np.linspace(0, len(data_set[0]), num=num_shards[data_index]+1, dtype=np.int32)
        for i in range(len(spacing)-1):
            ranges.append([spacing[i], spacing[i+1]])

        ### Create .tfrecord file
        for start, end in ranges:
            # get the tfrecord filename
            tfrecord_filename = '{0}_{1:05d}_{2:05d}.tfrecord'.format(file_prefixs[data_index], start, end-1)
            # Write axamples
            print('{0}: Creating'.format(tfrecord_filename))
            writer = tf.python_io.TFRecordWriter(os.path.join(data_url, tfrecord_filename))
            for index in range(start, end):
                # Get the txt data and label
                filename  = data_set[0][index]
                data      = np.loadtxt(filename)
                labelname = data_set[1][index]
                labelval  = data_set[2][index]
                # Remove the root directory of txt files
                fn_split = os.path.split(filename)
                filename = os.path.join(labelname, fn_split[1])
                # Add the txt data and label to a example
                    # filename : the url of txt file
                    # data_raw : the txt data
                    # labelname: the label name
                    # labelval : the label value
                example = tf.train.Example(
                                features = tf.train.Features(
                                    feature = {'filename' :tf.train.Feature(bytes_list = tf.train.BytesList(value = [str.encode(filename)])), 
                                               'data_raw' :tf.train.Feature(float_list = tf.train.FloatList(value = data)), 
                                               'labelname':tf.train.Feature(bytes_list = tf.train.BytesList(value = [str.encode(labelname)])), 
                                               'labelval' :tf.train.Feature(int64_list = tf.train.Int64List(value = [labelval])), 
                                            }))
                # Write to the .tfrecord file
                writer.write(example.SerializeToString())
            writer.close()
            print('{0}: Created'.format(tfrecord_filename))

    ### Write readme.txt
    with open(os.path.join(data_url, 'readme.txt'), 'w') as f:
        # -------------------- Created time -------------------- #
        f.write('Created at {0}\n\n'.format(datetime.datetime.now()))

        # -------------------- Tfrecord format -------------------- #
        f.write('Tfrecord format: \n')
        f.write('\tfilename  bytes  Filenames that have been removed root directory. \n')
        f.write('\tdata_raw  float  A vector data. \n')
        f.write('\tlabelname bytes  Label name. \n')
        f.write('\tlabelval  int    Label value that starts from 0. \n\n')

        # -------------------- Txt url -------------------- #
        f.write('Txt url: {0}\n\n'.format(txt_url))

        # -------------------- Txt infos -------------------- #
        f.write('Txt infos [x, y, z, type]: {0}\n\n'.format(txt_infos))

        # -------------------- Data infos -------------------- #
        ### Count the number of samples.
        data_infos  = get_data_info(data_sets , unique_labels)
        train_infos = get_data_info(train_sets, unique_labels)
        vld_infos   = get_data_info(vld_sets  , unique_labels)
        test_infos  = get_data_info(test_sets , unique_labels)

        ### Write data distribution
        f.write('Data distribution (seed={0}, data_pct={1}): \n'.format(seed, data_pct+[100-sum(data_pct)]))
        f.write('\t{0:8} {1}\n'.format('All', data_infos[0]))
        for i, infos in enumerate([train_infos, vld_infos, test_infos]):
            f.write('\t{0:8} {1}\n'.format(file_prefixs[i], infos[0]))
        f.write('\n')

        ### Write data filenames
        for i, infos in enumerate([train_infos, vld_infos, test_infos]):
            f.write('{0}: \n'.format(file_prefixs[i]))
            for info in infos[1]:
                for filename in info:
                    f.write('{0}\n'.format(filename))
            f.write('\n')
    

# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    if False:
        write_img_tfrecords(r'datas\airbag\version1\Images_V1', ['OK', 'NG'], r'output\gestures\dnn\data', 
                            [224, 224, 1, 'L'], file_exts=['bmp', 'jpg', 'png', 'tif'], data_pct=[80, 0], 
                            file_prefixs=['train', 'vld', 'test'], num_shards=[1, 0, 1], seed=1012)

    if True:
        write_txt_tfrecords(r'datas\gesture\version1\Labels', 
                            ['Standing', 'Sitting', 'Walking', 'StandUp', 'SitDown', 'TurnBack'], 
                            r'output\gesture\dnn\data', [4500, 1, 1, 'float'], file_exts=['txt'], data_pct=[80, 0], 
                            file_prefixs=['train', 'vld', 'test'], num_shards=[1, 0, 1], seed=1012)



