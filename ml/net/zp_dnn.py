# -*- coding: utf-8 -*-

''' zp_dnn.py

    A DNN(Deep Neural Network) architecture. (Multilayer perceptron 多层感知器)

    Important:
        dnn_all: Switch dnn_5. 
        dnn_5  : A DNN architecture with 4 hidden layers. 

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import numpy as np
import tensorflow as tf
import tensorflow.contrib.slim as slim


# ---------------------------------------------------------------------------------------------------- #
# --------------------------------------- The dnn architecture --------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def dnn_all(_params, net_name, inputs, num_classes=2, is_training=True, dropout_keep_prob=0.5, reuse=tf.AUTO_REUSE, scope='dnn_all'):
    ''' Switch dnn_5. 

        dnn_5  : A DNN architecture with 4 hidden layers. 

        Args:
            _params          : The global parameters. 
            net_name         : The net name. dnn_5
            inputs           : The input data sets whose shape is [1 x 4500]. 
            num_classes      : The number of output classes. 
            is_training      : Is training, if yes, it will ignore dropout_keep_prob.  
            dropout_keep_prob: The value of dropout parameter. 
            reuse            : 
            scope            : 

        Return:
            outputs: The output of the net. 

        Raise:
            raise ValueError('The net_name is invalid. ')
    '''
    ### Call net
    if net_name == 'dnn_5':
        net = dnn_5(inputs, num_classes, is_training, dropout_keep_prob, reuse, scope)
    else:
        raise ValueError('The net_name is invalid. ')

    ### Return
    return net


def dnn_5(inputs, num_classes=6, is_training=True, dropout_keep_prob=0.8, reuse=tf.AUTO_REUSE, scope='dnn_5'):
    ''' A DNN architecture with 4 hidden layers. 

        input --> (hidden layer) x 4 --> output

        hidden_layer_notes = [4096, 1024, 256, 64]
        1 x 4500 --> 1 x 4096 --> 1 x 1024 --> 1 x 256 --> 1 x 64 --> 1 x num_output

        Args:
            inputs           : The input data sets whose shape likes [1 x 4500]. 
            num_classes      : The number of output classes. 
            is_training      : Is training, if yes, it will ignore dropout_keep_prob.  
            dropout_keep_prob: The value of dropout parameter. 
            reuse            : 
            scope            :

        Return:
            net: The output of the net which do not input tf.nn.softmax. 

        Raise:

    '''
    with tf.variable_scope(scope, 'dnn_5', [inputs], reuse=reuse):
        ### hidden_layer_notes
        hidden_layer_notes = [4096, 1024, 256, 64]

        ### 1: hidden layer 1
        # 1 x 4500 --> 1 x 4096
        with tf.variable_scope('hidden1'):
            net = slim.fully_connected(inputs, hidden_layer_notes[0], scope='fc')
            net = slim.dropout(net, dropout_keep_prob, is_training=is_training, scope='dropout')
            
        ### 2: hidden layer 2
        # 1 x 4096 --> 1 x 1024
        with tf.variable_scope('hidden2'):
            net = slim.fully_connected(net, hidden_layer_notes[1], scope='fc')
            net = slim.dropout(net, dropout_keep_prob, is_training=is_training, scope='dropout')

        ### 3: hidden layer 3
        # 1 x 1024 --> 1 x 256
        with tf.variable_scope('hidden3'):
            net = slim.fully_connected(net, hidden_layer_notes[2], scope='fc')
            net = slim.dropout(net, dropout_keep_prob, is_training=is_training, scope='dropout')

        ### 4: hidden layer 4
        # 1 x 256 --> 1 x 64
        with tf.variable_scope('hidden4'):
            net = slim.fully_connected(net, hidden_layer_notes[3], scope='fc')
            net = slim.dropout(net, dropout_keep_prob, is_training=is_training, scope='dropout')
            
        ### 5: output layer
        # 1 x 64 --> 1 x num_classes
        with tf.variable_scope('output'):
            net = slim.fully_connected(net, num_classes, activation_fn=None, scope='fc')

        ### return
        return net


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    inputs = tf.placeholder(dtype=tf.float32, shape=[None, 4500])

    outputs = dnn_5(inputs, num_classes=6, is_training=True, dropout_keep_prob=0.8, 
                    reuse=False, scope='dnn_5')
            
    pass



