# -*- coding: utf-8 -*-

''' params.py

    Global parameters. 

    Important:
        Airbag:
            params_airbag_cnn
            params_airbag_vggnet
            params_airbag_resnet_v1
            params_airbag_resnet_v2

        Gestures:

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import sys
sys.path.append('net')

import os
import numpy as np
import tensorflow as tf

from net import zp_dnn
from net import zp_cnn
from net import vggnet
from net import resnet_v1
from net import resnet_v2


# ---------------------------------------------------------------------------------------------------- #
# ---------------------------------------------- Airbag ---------------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
class params_airbag_cnn(object):
    ### System parameters
    PER_PROCESS_GPU_MEMORY_FRACTION = 0.6   # The gpu memory fraction per process.

    ### Flag
    IS_WRITE_TFRECORD = False       # Is write .tfrecord files?
    IS_TRAINING       =  True       # Is training net?
    IS_TEST           =  True       # Is test net?
    IS_DEBUG          = False       # Is debug?
    IS_OPTIMIZING     =  True       # Is optimizing?

    IS_WRITE_F_Ms     = False       # Is write feature maps?
    IS_WRITE_T_Vs     = False       # Is write trainable variables?
    IS_WRITE_PB       = False       # Is write pb model when PARAMS.IS_TEST == True? 

    ### Input data informations
    IMAGE_URL     = r'datas\airbag\version1\Images_V1'      # Image directory.
    LABEL_URL     = os.path.join(IMAGE_URL, 'label.txt')    # Label file.
    SHUFFLED_SEED = 1012                                    # Shuffled seed when generate the tfrecord files. 
    LABELNAMES    = []                                      # Lebel names ['Class1', 'Class2', ...]

    ### Project informations
    PROJECT_NAME = 'airbag'         # The reseached project name.
    NET_FUNC     = zp_cnn.cnn_all   # The called net. 
    NET1_NAME    = 'cnn'            # The called net name.
    NET2_NAME    = 'cnn_6'          # The called net name: cnn_6.

    ### Output --> Project --> Net1 --> data + log
    ###                             --> Net2 --> graph + model
    OUTPUT_URL  = 'output'                                  # Output root directory.
    PROJECT_URL = os.path.join(OUTPUT_URL, PROJECT_NAME)    # Project directory.
    NET1_URL    = os.path.join(PROJECT_URL, NET1_NAME)      # Net 1 directory.
    NET2_URL    = os.path.join(NET1_URL, NET2_NAME)         # Net 2 directory.
    DATA_URL    = os.path.join(NET1_URL, 'data')            # Data output directory.
    LOG_URL     = os.path.join(NET2_URL, 'log')             # Log output directory.
    GRAPH_URL   = os.path.join(NET2_URL, 'graph')           # Graph output directory.
    MODEL_URL   = os.path.join(NET2_URL, 'model')           # Model output directory.
    PB_URL      = os.path.join(NET2_URL, 'pb')              # PB modle output directory.
    ANLS_URL    = os.path.join(NET2_URL, 'anls')            # Analysis output directory.

    ### File informations
    FILE_EXTS    = ['bmp', 'jpg', 'png', 'tif']     # The valid image extensions.
    FILE_PREFIXS = ['train', 'vld', 'test']         # The prefix of TFRecord file names [training, validation, test].
    FILE_SHARDS  = [1, 0, 1]                        # The number of shards in TFRecord files [training, validation, test].

    ### Data informations
        # Image mode
            # 1 (1-bit pixels, black and white, stored with one pixel per byte)
            # L (8-bit pixels, black and white)
            # RGB (3x8-bit pixels, true color)
    DATA_PCTS   = [80, 0]               # The percentage of data sets [training sets, validation sets].
    IMAGE_INFOS = [256, 256, 1, 'L']    # The info of image [width, height, channel, mode].

    ### Image pre-processing
    # adjust params
    IS_IMG_STD        = [True]          # per_image_standardization : [Enabled]
    ADJUST_CONTRAST   = [False, 1.0]    # tf.image.adjust_contrast  : [Enabled, factor]
    ADJUST_BRIGHTNESS = [False, 0.0]    # tf.image.adjust_brightness: [Enabled, max_delta]
    ADJUST_IMG_PPROC  = [IS_IMG_STD, ADJUST_CONTRAST, ADJUST_BRIGHTNESS]
    # random params
    RANDOM_CONTRAST        = [False, 1.0, 1.0]  # tf.image.random_contrast       : [Enabled, lower, upper]
    RANDOM_BRIGHTNESS      = [False, 0.0]       # tf.image.random_brightness     : [Enabled, max_delta]
    RANDOM_FILP_LEFT_RIGHT = [False]            # tf.image.random_flip_left_right: [Enabled]
    RANDOM_FILP_UP_DOWN    = [False]            # tf.image.random_flip_up_down   : [Enabled]
    RANDOM_IMG_PPROC       = [RANDOM_CONTRAST, RANDOM_BRIGHTNESS, RANDOM_FILP_LEFT_RIGHT, RANDOM_FILP_UP_DOWN]

    ### Net parameters
    DROPOUT_KEEP_PROB = 0.8     # The net parameter: dropout.
    NUM_OUTPUT        = 2       # The number of output classes.

    ### Training parameters
    BATCH_SIZE    = 32          # The training parameter: batch size.
    TRAIN_STEP    = 15000       # The training parameter: train step.
    LEARNING_RATE = 0.001       # The training parameter: learning rate.

    ### Optimizer
    TRAIN_STEP_GROUP        = [step for step in range(7000, 15001, 1000)]   # The list of train step.
    BATCH_SIZE_GROUP        = [16, 32, 48]                                  # The list of batch size.
    LEARNING_RATE_GROUP     = [0.001, 0.005, 0.010]                         # The list of learning rate.
    DROPOUT_KEEP_PROB_GROUP = [0.8, 0.7, 0.9]                               # The list of dropout.

    def __init__(self):
        ''' Class __init__ function. 
        '''
        ### Get the labelname
        if not os.path.exists(self.LABEL_URL):
            raise ValueError('Cannot find the label file. ')
        self.LABELNAMES = [line.strip() for line in tf.gfile.FastGFile(self.LABEL_URL, 'r').readlines()]

        ### Create output folders
        if not os.path.exists(self.OUTPUT_URL) : os.mkdir(self.OUTPUT_URL)
        if not os.path.exists(self.PROJECT_URL): os.mkdir(self.PROJECT_URL)
        if not os.path.exists(self.NET1_URL)   : os.mkdir(self.NET1_URL)
        if not os.path.exists(self.NET2_URL)   : os.mkdir(self.NET2_URL)
        if not os.path.exists(self.DATA_URL)   : os.mkdir(self.DATA_URL)
        if not os.path.exists(self.LOG_URL)    : os.mkdir(self.LOG_URL)
        if not os.path.exists(self.GRAPH_URL)  : os.mkdir(self.GRAPH_URL)
        if not os.path.exists(self.MODEL_URL)  : os.mkdir(self.MODEL_URL)
        if not os.path.exists(self.PB_URL)     : os.mkdir(self.PB_URL)
        if not os.path.exists(self.ANLS_URL)   : os.mkdir(self.ANLS_URL)


class params_airbag_vggnet(object):
    ### System parameters
    PER_PROCESS_GPU_MEMORY_FRACTION = 0.6   # The gpu memory fraction per process.

    ### Flag
    IS_WRITE_TFRECORD = False       # Is write .tfrecord files?
    IS_TRAINING       =  True       # Is training net?
    IS_TEST           =  True       # Is test net?
    IS_DEBUG          = False       # Is debug?
    IS_OPTIMIZING     =  True       # Is optimizing?

    IS_WRITE_F_Ms     = False       # Is write feature maps?
    IS_WRITE_T_Vs     = False       # Is write trainable variables?
    IS_WRITE_PB       = False       # Is write pb model when PARAMS.IS_TEST == True? 

    ### Input data informations
    IMAGE_URL     = r'datas\airbag\version1\Images_V1'      # Image directory.
    LABEL_URL     = os.path.join(IMAGE_URL, 'label.txt')    # Label file.
    SHUFFLED_SEED = 1012                                    # Shuffled seed when generate the tfrecord files. 
    LABELNAMES    = []                                      # Lebel names ['Class1', 'Class2', ...]

    ### Project informations
    PROJECT_NAME = 'airbag'         # The reseached project name.
    NET_FUNC     = vggnet.vgg_all   # The called net. 
    NET1_NAME    = 'vggnet'         # The called net name.
    NET2_NAME    = 'vgg_d_16'       # The called net name: vgg_a_11, vgg_d_16, vgg_d_19.

    ### Output --> Project --> Net1 --> data + log
    ###                             --> Net2 --> graph + model
    OUTPUT_URL  = 'output'                                  # Output root directory.
    PROJECT_URL = os.path.join(OUTPUT_URL, PROJECT_NAME)    # Project directory.
    NET1_URL    = os.path.join(PROJECT_URL, NET1_NAME)      # Net 1 directory.
    NET2_URL    = os.path.join(NET1_URL, NET2_NAME)         # Net 2 directory.
    DATA_URL    = os.path.join(NET1_URL, 'data')            # Data output directory.
    LOG_URL     = os.path.join(NET2_URL, 'log')             # Log output directory.
    GRAPH_URL   = os.path.join(NET2_URL, 'graph')           # Graph output directory.
    MODEL_URL   = os.path.join(NET2_URL, 'model')           # Model output directory.
    PB_URL      = os.path.join(NET2_URL, 'pb')              # PB modle output directory.
    ANLS_URL    = os.path.join(NET2_URL, 'anls')            # Analysis output directory.

    ### File informations
    FILE_EXTS    = ['bmp', 'jpg', 'png', 'tif']     # The valid image extensions.
    FILE_PREFIXS = ['train', 'vld', 'test']         # The prefix of TFRecord file names [training, validation, test].
    FILE_SHARDS  = [1, 0, 1]                        # The number of shards in TFRecord files [training, validation, test].

    ### Data informations
        # Image mode
            # 1 (1-bit pixels, black and white, stored with one pixel per byte)
            # L (8-bit pixels, black and white)
            # RGB (3x8-bit pixels, true color)
    DATA_PCTS   = [80, 0]               # The percentage of data sets [training sets, validation sets].
    IMAGE_INFOS = [224, 224, 1, 'L']    # The info of image [width, height, channel, mode].

    ### Image pre-processing
    # adjust params
    IS_IMG_STD        = [True]          # per_image_standardization : [Enabled]
    ADJUST_CONTRAST   = [False, 1.0]    # tf.image.adjust_contrast  : [Enabled, factor]
    ADJUST_BRIGHTNESS = [False, 0.0]    # tf.image.adjust_brightness: [Enabled, max_delta]
    ADJUST_IMG_PPROC  = [IS_IMG_STD, ADJUST_CONTRAST, ADJUST_BRIGHTNESS]
    # random params
    RANDOM_CONTRAST        = [False, 1.0, 1.0]  # tf.image.random_contrast       : [Enabled, lower, upper]
    RANDOM_BRIGHTNESS      = [False, 0.0]       # tf.image.random_brightness     : [Enabled, max_delta]
    RANDOM_FILP_LEFT_RIGHT = [False]            # tf.image.random_flip_left_right: [Enabled]
    RANDOM_FILP_UP_DOWN    = [False]            # tf.image.random_flip_up_down   : [Enabled]
    RANDOM_IMG_PPROC       = [RANDOM_CONTRAST, RANDOM_BRIGHTNESS, RANDOM_FILP_LEFT_RIGHT, RANDOM_FILP_UP_DOWN]

    ### Net parameters
    DROPOUT_KEEP_PROB = 0.8     # The net parameter: dropout.
    NUM_OUTPUT        = 2       # The number of output classes.

    ### Training parameters
    BATCH_SIZE    = 32          # The training parameter: batch size.
    TRAIN_STEP    = 15000       # The training parameter: train step.
    LEARNING_RATE = 0.001       # The training parameter: learning rate.

    ### Optimizer
    TRAIN_STEP_GROUP        = [step for step in range(7000, 15001, 1000)]   # The list of train step.
    BATCH_SIZE_GROUP        = [16, 32, 48]                                  # The list of batch size.
    LEARNING_RATE_GROUP     = [0.001, 0.005, 0.010]                         # The list of learning rate.
    DROPOUT_KEEP_PROB_GROUP = [0.8, 0.7, 0.9]                               # The list of dropout.

    def __init__(self):
        ''' Class __init__ function. 
        '''
        ### Get the labelname
        if not os.path.exists(self.LABEL_URL):
            raise ValueError('Cannot find the label file. ')
        self.LABELNAMES = [line.strip() for line in tf.gfile.FastGFile(self.LABEL_URL, 'r').readlines()]

        ### Create output folders
        if not os.path.exists(self.OUTPUT_URL) : os.mkdir(self.OUTPUT_URL)
        if not os.path.exists(self.PROJECT_URL): os.mkdir(self.PROJECT_URL)
        if not os.path.exists(self.NET1_URL)   : os.mkdir(self.NET1_URL)
        if not os.path.exists(self.NET2_URL)   : os.mkdir(self.NET2_URL)
        if not os.path.exists(self.DATA_URL)   : os.mkdir(self.DATA_URL)
        if not os.path.exists(self.LOG_URL)    : os.mkdir(self.LOG_URL)
        if not os.path.exists(self.GRAPH_URL)  : os.mkdir(self.GRAPH_URL)
        if not os.path.exists(self.MODEL_URL)  : os.mkdir(self.MODEL_URL)
        if not os.path.exists(self.PB_URL)     : os.mkdir(self.PB_URL)
        if not os.path.exists(self.ANLS_URL)   : os.mkdir(self.ANLS_URL)


class params_airbag_resnet_v1(object):
    ### System parameters
    PER_PROCESS_GPU_MEMORY_FRACTION = 0.6   # The gpu memory fraction per process.

    ### Flag
    IS_WRITE_TFRECORD = False       # Is write .tfrecord files?
    IS_TRAINING       =  True       # Is training net?
    IS_TEST           =  True       # Is test net?
    IS_DEBUG          = False       # Is debug?
    IS_OPTIMIZING     =  True       # Is optimizing?

    IS_WRITE_F_Ms     = False       # Is write feature maps?
    IS_WRITE_T_Vs     = False       # Is write trainable variables?
    IS_WRITE_PB       = False       # Is write pb model when PARAMS.IS_TEST == True? 

    ### Input data informations
    IMAGE_URL     = r'datas\airbag\version1\Images_V1'      # Image directory.
    LABEL_URL     = os.path.join(IMAGE_URL, 'label.txt')    # Label file.
    SHUFFLED_SEED = 1012                                    # Shuffled seed when generate the tfrecord files. 
    LABELNAMES    = []                                      # Lebel names ['Class1', 'Class2', ...]

    ### Project informations
    PROJECT_NAME = 'airbag'                 # The reseached project name.
    NET_FUNC     = resnet_v1.resnet_all     # The called net. 
    NET1_NAME    = 'resnet_v1'              # The called net name.
    NET2_NAME    = 'resnet_v1_34'           # The called net name: resnet_v1_18, resnet_v1_34, resnet_v1_50, resnet_v1_101, resnet_v1_152.

    ### Output --> Project --> Net1 --> data + log
    ###                             --> Net2 --> graph + model
    OUTPUT_URL  = 'output'                                  # Output root directory.
    PROJECT_URL = os.path.join(OUTPUT_URL, PROJECT_NAME)    # Project directory.
    NET1_URL    = os.path.join(PROJECT_URL, NET1_NAME)      # Net 1 directory.
    NET2_URL    = os.path.join(NET1_URL, NET2_NAME)         # Net 2 directory.
    DATA_URL    = os.path.join(NET1_URL, 'data')            # Data output directory.
    LOG_URL     = os.path.join(NET2_URL, 'log')             # Log output directory.
    GRAPH_URL   = os.path.join(NET2_URL, 'graph')           # Graph output directory.
    MODEL_URL   = os.path.join(NET2_URL, 'model')           # Model output directory.
    PB_URL      = os.path.join(NET2_URL, 'pb')              # PB modle output directory.
    ANLS_URL    = os.path.join(NET2_URL, 'anls')            # Analysis output directory.

    ### File informations
    FILE_EXTS    = ['bmp', 'jpg', 'png', 'tif']     # The valid image extensions.
    FILE_PREFIXS = ['train', 'vld', 'test']         # The prefix of TFRecord file names [training, validation, test].
    FILE_SHARDS  = [1, 0, 1]                        # The number of shards in TFRecord files [training, validation, test].

    ### Data informations
        # Image mode
            # 1 (1-bit pixels, black and white, stored with one pixel per byte)
            # L (8-bit pixels, black and white)
            # RGB (3x8-bit pixels, true color)
    DATA_PCTS   = [80, 0]               # The percentage of data sets [training sets, validation sets].
    IMAGE_INFOS = [224, 224, 1, 'L']    # The info of image [width, height, channel, mode].

    ### Image pre-processing
    # adjust params
    IS_IMG_STD        = [True]          # per_image_standardization : [Enabled]
    ADJUST_CONTRAST   = [False, 1.0]    # tf.image.adjust_contrast  : [Enabled, factor]
    ADJUST_BRIGHTNESS = [False, 0.0]    # tf.image.adjust_brightness: [Enabled, max_delta]
    ADJUST_IMG_PPROC  = [IS_IMG_STD, ADJUST_CONTRAST, ADJUST_BRIGHTNESS]
    # random params
    RANDOM_CONTRAST        = [False, 1.0, 1.0]  # tf.image.random_contrast       : [Enabled, lower, upper]
    RANDOM_BRIGHTNESS      = [False, 0.0]       # tf.image.random_brightness     : [Enabled, max_delta]
    RANDOM_FILP_LEFT_RIGHT = [False]            # tf.image.random_flip_left_right: [Enabled]
    RANDOM_FILP_UP_DOWN    = [False]            # tf.image.random_flip_up_down   : [Enabled]
    RANDOM_IMG_PPROC       = [RANDOM_CONTRAST, RANDOM_BRIGHTNESS, RANDOM_FILP_LEFT_RIGHT, RANDOM_FILP_UP_DOWN]

    ### Net parameters
    DROPOUT_KEEP_PROB = 0.8     # The net parameter: dropout.
    NUM_OUTPUT        = 2       # The number of output classes.

    ### Training parameters
    BATCH_SIZE    = 32          # The training parameter: batch size.
    TRAIN_STEP    = 15000       # The training parameter: train step.
    LEARNING_RATE = 0.001       # The training parameter: learning rate.

    ### Optimizer
    TRAIN_STEP_GROUP        = [step for step in range(7000, 15001, 1000)]   # The list of train step.
    BATCH_SIZE_GROUP        = [16, 32, 48]                                  # The list of batch size.
    LEARNING_RATE_GROUP     = [0.001, 0.005, 0.010]                         # The list of learning rate.
    DROPOUT_KEEP_PROB_GROUP = [0.8, 0.7, 0.9]                               # The list of dropout.

    def __init__(self):
        ''' Class __init__ function. 
        '''
        ### Get the labelname
        if not os.path.exists(self.LABEL_URL):
            raise ValueError('Cannot find the label file. ')
        self.LABELNAMES = [line.strip() for line in tf.gfile.FastGFile(self.LABEL_URL, 'r').readlines()]

        ### Create output folders
        if not os.path.exists(self.OUTPUT_URL) : os.mkdir(self.OUTPUT_URL)
        if not os.path.exists(self.PROJECT_URL): os.mkdir(self.PROJECT_URL)
        if not os.path.exists(self.NET1_URL)   : os.mkdir(self.NET1_URL)
        if not os.path.exists(self.NET2_URL)   : os.mkdir(self.NET2_URL)
        if not os.path.exists(self.DATA_URL)   : os.mkdir(self.DATA_URL)
        if not os.path.exists(self.LOG_URL)    : os.mkdir(self.LOG_URL)
        if not os.path.exists(self.GRAPH_URL)  : os.mkdir(self.GRAPH_URL)
        if not os.path.exists(self.MODEL_URL)  : os.mkdir(self.MODEL_URL)
        if not os.path.exists(self.PB_URL)     : os.mkdir(self.PB_URL)
        if not os.path.exists(self.ANLS_URL)   : os.mkdir(self.ANLS_URL)


class params_airbag_resnet_v2(object):
    ### System parameters
    PER_PROCESS_GPU_MEMORY_FRACTION = 0.6   # The gpu memory fraction per process.

    ### Flag
    IS_WRITE_TFRECORD = False       # Is write .tfrecord files?
    IS_TRAINING       =  True       # Is training net?
    IS_TEST           =  True       # Is test net?
    IS_DEBUG          = False       # Is debug?
    IS_OPTIMIZING     =  True       # Is optimizing?

    IS_WRITE_F_Ms     = False       # Is write feature maps?
    IS_WRITE_T_Vs     = False       # Is write trainable variables?
    IS_WRITE_PB       = False       # Is write pb model when PARAMS.IS_TEST == True? 

    ### Input data informations
    IMAGE_URL     = r'datas\airbag\version1\Images_V1'      # Image directory.
    LABEL_URL     = os.path.join(IMAGE_URL, 'label.txt')    # Label file.
    SHUFFLED_SEED = 1012                                    # Shuffled seed when generate the tfrecord files. 
    LABELNAMES    = []                                      # Lebel names ['Class1', 'Class2', ...]

    ### Project informations
    PROJECT_NAME = 'airbag'                 # The reseached project name.
    NET_FUNC     = resnet_v2.resnet_all     # The called net. 
    NET1_NAME    = 'resnet_v2'              # The called net name.
    NET2_NAME    = 'resnet_v2_34'           # The called net name: resnet_v2_18, resnet_v2_34, resnet_v2_50, resnet_v2_101, resnet_v2_152.

    ### Output --> Project --> Net1 --> data + log
    ###                             --> Net2 --> graph + model
    OUTPUT_URL  = 'output'                                  # Output root directory.
    PROJECT_URL = os.path.join(OUTPUT_URL, PROJECT_NAME)    # Project directory.
    NET1_URL    = os.path.join(PROJECT_URL, NET1_NAME)      # Net 1 directory.
    NET2_URL    = os.path.join(NET1_URL, NET2_NAME)         # Net 2 directory.
    DATA_URL    = os.path.join(NET1_URL, 'data')            # Data output directory.
    LOG_URL     = os.path.join(NET2_URL, 'log')             # Log output directory.
    GRAPH_URL   = os.path.join(NET2_URL, 'graph')           # Graph output directory.
    MODEL_URL   = os.path.join(NET2_URL, 'model')           # Model output directory.
    PB_URL      = os.path.join(NET2_URL, 'pb')              # PB modle output directory.
    ANLS_URL    = os.path.join(NET2_URL, 'anls')            # Analysis output directory.

    ### File informations
    FILE_EXTS    = ['bmp', 'jpg', 'png', 'tif']     # The valid image extensions.
    FILE_PREFIXS = ['train', 'vld', 'test']         # The prefix of TFRecord file names [training, validation, test].
    FILE_SHARDS  = [1, 0, 1]                        # The number of shards in TFRecord files [training, validation, test].

    ### Data informations
        # Image mode
            # 1 (1-bit pixels, black and white, stored with one pixel per byte)
            # L (8-bit pixels, black and white)
            # RGB (3x8-bit pixels, true color)
    DATA_PCTS   = [80, 0]               # The percentage of data sets [training sets, validation sets].
    IMAGE_INFOS = [224, 224, 1, 'L']    # The info of image [width, height, channel, mode].

    ### Image pre-processing
    # adjust params
    IS_IMG_STD        = [True]          # per_image_standardization : [Enabled]
    ADJUST_CONTRAST   = [False, 1.0]    # tf.image.adjust_contrast  : [Enabled, factor]
    ADJUST_BRIGHTNESS = [False, 0.0]    # tf.image.adjust_brightness: [Enabled, max_delta]
    ADJUST_IMG_PPROC  = [IS_IMG_STD, ADJUST_CONTRAST, ADJUST_BRIGHTNESS]
    # random params
    RANDOM_CONTRAST        = [False, 1.0, 1.0]  # tf.image.random_contrast       : [Enabled, lower, upper]
    RANDOM_BRIGHTNESS      = [False, 0.0]       # tf.image.random_brightness     : [Enabled, max_delta]
    RANDOM_FILP_LEFT_RIGHT = [False]            # tf.image.random_flip_left_right: [Enabled]
    RANDOM_FILP_UP_DOWN    = [False]            # tf.image.random_flip_up_down   : [Enabled]
    RANDOM_IMG_PPROC       = [RANDOM_CONTRAST, RANDOM_BRIGHTNESS, RANDOM_FILP_LEFT_RIGHT, RANDOM_FILP_UP_DOWN]

    ### Net parameters
    DROPOUT_KEEP_PROB = 0.8     # The net parameter: dropout.
    NUM_OUTPUT        = 2       # The number of output classes.

    ### Training parameters
    BATCH_SIZE    = 32          # The training parameter: batch size.
    TRAIN_STEP    = 15000       # The training parameter: train step.
    LEARNING_RATE = 0.001       # The training parameter: learning rate.

    ### Optimizer
    TRAIN_STEP_GROUP        = [step for step in range(7000, 15001, 1000)]   # The list of train step.
    BATCH_SIZE_GROUP        = [16, 32, 48]                                  # The list of batch size.
    LEARNING_RATE_GROUP     = [0.001, 0.005, 0.010]                         # The list of learning rate.
    DROPOUT_KEEP_PROB_GROUP = [0.8, 0.7, 0.9]                               # The list of dropout.

    def __init__(self):
        ''' Class __init__ function. 
        '''
        ### Get the labelname
        if not os.path.exists(self.LABEL_URL):
            raise ValueError('Cannot find the label file. ')
        self.LABELNAMES = [line.strip() for line in tf.gfile.FastGFile(self.LABEL_URL, 'r').readlines()]

        ### Create output folders
        if not os.path.exists(self.OUTPUT_URL) : os.mkdir(self.OUTPUT_URL)
        if not os.path.exists(self.PROJECT_URL): os.mkdir(self.PROJECT_URL)
        if not os.path.exists(self.NET1_URL)   : os.mkdir(self.NET1_URL)
        if not os.path.exists(self.NET2_URL)   : os.mkdir(self.NET2_URL)
        if not os.path.exists(self.DATA_URL)   : os.mkdir(self.DATA_URL)
        if not os.path.exists(self.LOG_URL)    : os.mkdir(self.LOG_URL)
        if not os.path.exists(self.GRAPH_URL)  : os.mkdir(self.GRAPH_URL)
        if not os.path.exists(self.MODEL_URL)  : os.mkdir(self.MODEL_URL)
        if not os.path.exists(self.PB_URL)     : os.mkdir(self.PB_URL)
        if not os.path.exists(self.ANLS_URL)   : os.mkdir(self.ANLS_URL)


# ---------------------------------------------------------------------------------------------------- #
# --------------------------------------------- Gesture ---------------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
class params_gesture_dnn(object):
    ### System parameters
    PER_PROCESS_GPU_MEMORY_FRACTION = 0.6   # The gpu memory fraction per process.

    ### Flag
    IS_WRITE_TFRECORD = False       # Is write .tfrecord files?
    IS_TRAINING       =  True       # Is training net?
    IS_TEST           =  True       # Is test net?
    IS_DEBUG          = False       # Is debug?
    IS_OPTIMIZING     =  True       # Is optimizing?

    IS_WRITE_F_Ms     = False       # Is write feature maps?
    IS_WRITE_T_Vs     = False       # Is write trainable variables?
    IS_WRITE_PB       = False       # Is write pb model when PARAMS.IS_TEST == True? 

    ### Input data informations
    TXT_URL       = r'datas\gesture\version1\Labels'        # Txt directory.
    LABEL_URL     = os.path.join(TXT_URL, 'label.txt')      # Label file.
    SHUFFLED_SEED = 1012                                    # Shuffled seed when generate the tfrecord files. 
    LABELNAMES    = []                                      # Lebel names ['Class1', 'Class2', ...]

    ### Project informations
    PROJECT_NAME = 'gesture'            # The reseached project name.
    NET_FUNC     = zp_dnn.dnn_all       # The called net. 
    NET1_NAME    = 'dnn'                # The called net name.
    NET2_NAME    = 'dnn_5'              # The called net name: dnn_5.

    ### Output --> Project --> Net1 --> data + log
    ###                             --> Net2 --> graph + model
    OUTPUT_URL  = 'output'                                  # Output root directory.
    PROJECT_URL = os.path.join(OUTPUT_URL, PROJECT_NAME)    # Project directory.
    NET1_URL    = os.path.join(PROJECT_URL, NET1_NAME)      # Net 1 directory.
    NET2_URL    = os.path.join(NET1_URL, NET2_NAME)         # Net 2 directory.
    DATA_URL    = os.path.join(NET1_URL, 'data')            # Data output directory.
    LOG_URL     = os.path.join(NET2_URL, 'log')             # Log output directory.
    GRAPH_URL   = os.path.join(NET2_URL, 'graph')           # Graph output directory.
    MODEL_URL   = os.path.join(NET2_URL, 'model')           # Model output directory.
    PB_URL      = os.path.join(NET2_URL, 'pb')              # PB modle output directory.
    ANLS_URL    = os.path.join(NET2_URL, 'anls')            # Analysis output directory.

    ### File informations
    FILE_EXTS    = ['txt']                          # The valid image extensions.
    FILE_PREFIXS = ['train', 'vld', 'test']         # The prefix of TFRecord file names [training, validation, test].
    FILE_SHARDS  = [1, 0, 1]                        # The number of shards in TFRecord files [training, validation, test].

    ### Data informations
    DATA_PCTS = [80, 0]                   # The percentage of data sets [training sets, validation sets].
    TXT_INFOS = [4500, 1, 1, 'float']     # The info of txt [x, y, z, type].

    ### Net parameters
    DROPOUT_KEEP_PROB = 0.8     # The net parameter: dropout.
    NUM_OUTPUT        = 6       # The number of output classes.

    ### Training parameters
    BATCH_SIZE    = 32          # The training parameter: batch size.
    TRAIN_STEP    = 15000       # The training parameter: train step.
    LEARNING_RATE = 0.001       # The training parameter: learning rate.

    ### Optimizer
    TRAIN_STEP_GROUP        = [step for step in range(7000, 15001, 1000)]   # The list of train step.
    BATCH_SIZE_GROUP        = [16, 32, 48]                                  # The list of batch size.
    LEARNING_RATE_GROUP     = [0.001, 0.005, 0.010]                         # The list of learning rate.
    DROPOUT_KEEP_PROB_GROUP = [0.8, 0.7, 0.9]                               # The list of dropout.

    def __init__(self):
        ''' Class __init__ function. 
        '''
        ### Get the labelname
        if not os.path.exists(self.LABEL_URL):
            raise ValueError('Cannot find the label file. ')
        self.LABELNAMES = [line.strip() for line in tf.gfile.FastGFile(self.LABEL_URL, 'r').readlines()]

        ### Create output folders
        if not os.path.exists(self.OUTPUT_URL) : os.mkdir(self.OUTPUT_URL)
        if not os.path.exists(self.PROJECT_URL): os.mkdir(self.PROJECT_URL)
        if not os.path.exists(self.NET1_URL)   : os.mkdir(self.NET1_URL)
        if not os.path.exists(self.NET2_URL)   : os.mkdir(self.NET2_URL)
        if not os.path.exists(self.DATA_URL)   : os.mkdir(self.DATA_URL)
        if not os.path.exists(self.LOG_URL)    : os.mkdir(self.LOG_URL)
        if not os.path.exists(self.GRAPH_URL)  : os.mkdir(self.GRAPH_URL)
        if not os.path.exists(self.MODEL_URL)  : os.mkdir(self.MODEL_URL)
        if not os.path.exists(self.PB_URL)     : os.mkdir(self.PB_URL)
        if not os.path.exists(self.ANLS_URL)   : os.mkdir(self.ANLS_URL)



