# -*- coding: utf-8 -*-

''' main_gesture.py

    A project about judging the human gestures.

    Important:
        zp_dnn

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import sys
sys.path.append('func')

import os
import time
import logging
import datetime
import numpy as np
import tensorflow as tf
import tensorflow.contrib.slim as slim

import params
from func import log_handle
from func import img_handle
from func import wdata_handle
from func import rdata_handle


# ---------------------------------------------------------------------------------------------------- #
# ------------------------------------- Define global parameters ------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
### Global parameters
PARAMS = params.params_gesture_dnn()

### Update parameters
PARAMS.IS_WRITE_TFRECORD = False    # Is write .tfrecord files?
PARAMS.IS_TRAINING       =  True    # Is training net?
PARAMS.IS_TEST           =  True    # Is test net?
PARAMS.IS_DEBUG          = False    # Is debug?
PARAMS.IS_OPTIMIZING     = False    # Is optimizing?

PARAMS.IS_WRITE_F_Ms     = False    # Is write feature maps?
PARAMS.IS_WRITE_T_Vs     = False    # Is write trainable variables?
PARAMS.IS_WRITE_PB       = False    # Is write pb model when PARAMS.IS_TEST == True? 

### Training params
PARAMS.TRAIN_STEP        = 15000    # The training parameter: train step.
PARAMS.BATCH_SIZE        = 32       # The training parameter: batch size.
PARAMS.LEARNING_RATE     = 0.0001   # The training parameter: learning rate.
PARAMS.DROPOUT_KEEP_PROB = 1.0      # The net parameter: dropout.

PARAMS.TRAIN_STEP_GROUP        = [step for step in range(10000, 15001, 1000)]    # The list of train step.
PARAMS.BATCH_SIZE_GROUP        = [32]                                           # The list of batch size.
PARAMS.LEARNING_RATE_GROUP     = [0.0001]                                        # The list of learning rate.
PARAMS.DROPOUT_KEEP_PROB_GROUP = [1.0]                                          # The list of dropout.    


# ---------------------------------------------------------------------------------------------------- #
# ------------------------------------------ Main function ------------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def run_net(net_func, is_optimizing=False, train_step_group=[10000, 15000, 20000], batch_size=16, learning_rate=0.001, dropout_keep_prob=1.0):
    # ---------------------------------------------------------------- #
    # -------------------- Assign training params -------------------- #
    # ---------------------------------------------------------------- #
    ### Assign training param
    if not is_optimizing:
        train_step_group  = [PARAMS.TRAIN_STEP]
        batch_size        = PARAMS.BATCH_SIZE
        learning_rate     = PARAMS.LEARNING_RATE
        dropout_keep_prob = PARAMS.DROPOUT_KEEP_PROB
        param_name = 'not_optimizer'
    else:
        PARAMS.TRAIN_STEP        = np.max(train_step_group)
        PARAMS.BATCH_SIZE        = batch_size
        PARAMS.LEARNING_RATE     = learning_rate
        PARAMS.DROPOUT_KEEP_PROB = dropout_keep_prob
        param_name = 'bs={0} lr={1} do={2}'.format(batch_size, learning_rate, dropout_keep_prob)
        
    ### Log training infos
    log_handle.add_title(logger, 'Run net at {0}'.format(datetime.datetime.now().strftime('%Y-%m-%d')))
    log_handle.add_title(logger,        'train_step: {0}'.format(PARAMS.TRAIN_STEP)       , mode=3)
    log_handle.add_title(logger,        'batch_size: {0}'.format(PARAMS.BATCH_SIZE)       , mode=3)
    log_handle.add_title(logger,     'learning_rate: {0}'.format(PARAMS.LEARNING_RATE)    , mode=3)
    log_handle.add_title(logger, 'dropout_keep_prob: {0}'.format(PARAMS.DROPOUT_KEEP_PROB), mode=3)

    # ---------------------------------------------------------------- #
    # --------------------- Write tfrecord files --------------------- #
    # ---------------------------------------------------------------- #
    if PARAMS.IS_WRITE_TFRECORD:
        wdata_handle.write_txt_tfrecords(PARAMS.TXT_URL, PARAMS.LABELNAMES, PARAMS.DATA_URL, PARAMS.TXT_INFOS, 
                                            file_exts=PARAMS.FILE_EXTS, data_pct=PARAMS.DATA_PCTS, 
                                            file_prefixs=PARAMS.FILE_PREFIXS, num_shards=PARAMS.FILE_SHARDS, 
                                            seed=PARAMS.SHUFFLED_SEED)        
    
    # ---------------------------------------------------------------- #
    # ------------------------ Define tensor ------------------------- #
    # ---------------------------------------------------------------- #
    if PARAMS.IS_TRAINING or PARAMS.IS_TEST:
        with tf.Graph().as_default() as graph:
            # -------------------- Define training data -------------------- #
            if PARAMS.IS_TRAINING:
                ### Get train data batches, using shuffle_batch
                train_data = rdata_handle.get_txt_batches(PARAMS.DATA_URL, PARAMS.FILE_PREFIXS[0], PARAMS.BATCH_SIZE, PARAMS.TXT_INFOS, 
                                                            is_training=True, num_threads=4, capacity=1500, min_after_dequeue=128)
                train_images = tf.cast(train_data['data_raw'], tf.float32)
                train_labels = tf.one_hot(train_data['labelval'], PARAMS.NUM_OUTPUT)

            # -------------------- Define test data -------------------- #
            if PARAMS.IS_TEST:
                ### Get test batches, set shuffle_batch equals False
                test_data = rdata_handle.get_txt_batches(PARAMS.DATA_URL, PARAMS.FILE_PREFIXS[2], 1, PARAMS.TXT_INFOS, 
                                                            is_training=False)
                test_images = tf.cast(test_data['data_raw'], tf.float32)
                test_labels = tf.one_hot(test_data['labelval'], PARAMS.NUM_OUTPUT)
                                                    
            # -------------------- Define net -------------------- #
            ### Define placeholder variables
            is_training  = tf.placeholder(tf.bool, name='is_training')
            batch_images = tf.placeholder(tf.float32, shape=[None]+PARAMS.TXT_INFOS[0:1], name='batch_images')
            batch_labels = tf.placeholder(tf.float32, shape=[None, PARAMS.NUM_OUTPUT] , name='batch_labels')

            ### Call net
            logits = net_func(PARAMS.NET2_NAME, batch_images, num_classes=PARAMS.NUM_OUTPUT, 
                                is_training=is_training, dropout_keep_prob=PARAMS.DROPOUT_KEEP_PROB, 
                                reuse=False, scope=PARAMS.NET2_NAME)

            ### Loss function
            loss = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(labels=batch_labels, logits=logits))

            ### Optimizer
            train = tf.train.AdamOptimizer(learning_rate=PARAMS.LEARNING_RATE, beta1=0.9, beta2=0.999, epsilon=1e-8).minimize(loss)

            # -------------------- Define accuracy -------------------- #
            if PARAMS.IS_TRAINING:
                ### Training Accuracy
                train_pct = tf.nn.softmax(logits, name='train_pct')
                train_acc = tf.reduce_mean(tf.cast(tf.equal(tf.argmax(batch_labels, 1), tf.argmax(train_pct, 1)), tf.float32))

            if PARAMS.IS_TEST:
                ### Test accuracy
                test_pct = tf.nn.softmax(logits, name='test_pct')
                test_acc = tf.reduce_mean(tf.cast(tf.equal(tf.argmax(batch_labels, 1), tf.argmax(test_pct, 1)), tf.float32))

            # -------------------- Show in tensorboard -------------------- #
            if PARAMS.IS_TRAINING:
                ### Training results
                tf.summary.scalar('train_loss', loss, collections=['zp_train'])
                tf.summary.scalar('train_acc', train_acc, collections=['zp_train'])
                train_merged     = tf.summary.merge_all('zp_train')

            ### Net trainable variables
            if PARAMS.IS_WRITE_T_Vs:
                net_vars = slim.get_trainable_variables()
                for net_var in net_vars:
                    tf.summary.histogram(str(net_var.name).replace(':', '_'), net_var, collections=['zp_net'])
                net_merged = tf.summary.merge_all('zp_net')

        # ---------------------------------------------------------------- #
        # --------------------------- Running ---------------------------- #
        # ---------------------------------------------------------------- #
        gpu_options = tf.GPUOptions(per_process_gpu_memory_fraction=PARAMS.PER_PROCESS_GPU_MEMORY_FRACTION)
        with tf.Session(graph=graph, config=tf.ConfigProto(gpu_options=gpu_options)) as sess:
            ### Initialization
            sess.run(tf.local_variables_initializer())
            sess.run(tf.global_variables_initializer())

            ### Create tensorboard and add graph
            tb_writer = tf.summary.FileWriter(os.path.join(PARAMS.GRAPH_URL, param_name), graph=sess.graph)

            ### We need to add this code because we use queue to read tfrecord files.
            coord   = tf.train.Coordinator()
            threads = tf.train.start_queue_runners(sess=sess, coord=coord)

            # -------------------- Training -------------------- #
            if PARAMS.IS_TRAINING:
                ### Log
                log_handle.add_title(logger, 'Training at {0}'.format(datetime.datetime.now().strftime('%Y-%m-%d')), mode=2)

                ### Create model saver
                train_saver = tf.train.Saver(max_to_keep=len(train_step_group))

                ### Training
                for step in range(PARAMS.TRAIN_STEP+1):
                    ### Get training data and generate the feed_dict
                    run_train_data, run_train_images, run_train_labels = sess.run([train_data, train_images, train_labels])
                    feed_dict = {is_training:True, batch_images:run_train_images, batch_labels:run_train_labels}

                    ### Training the net
                    run_train, run_loss, run_train_merged = sess.run([train, loss, train_merged], feed_dict=feed_dict)
                    tb_writer.add_summary(run_train_merged, step)

                    ### Output train accuracy to console
                    if step % 100 == 0:
                        ### Write the trainable variables to TensorBoard
                        if PARAMS.IS_WRITE_T_Vs:
                            tb_writer.add_summary(sess.run(net_merged), step)

                        ### Log the training loss and training accuracy every 100 steps
                        run_train_acc = sess.run(train_acc, feed_dict=feed_dict)
                        logger.info('step {0:05d}: loss = {1:.8f}, acc = {2:.4f} %'.format(step, run_loss, run_train_acc*100.0))

                    ### Write model
                    if step in train_step_group:
                        train_saver.save(sess, os.path.join(PARAMS.MODEL_URL, param_name, 'model.ckpt'), global_step=step)

            # -------------------- Test -------------------- #
            if PARAMS.IS_TEST:
                ### Log
                log_handle.add_title(logger, 'Test at {0}'.format(datetime.datetime.now().strftime('%Y-%m-%d')), mode=2)

                for train_step in train_step_group:
                    ### Read model
                    test_saver = tf.train.Saver()
                    if not is_optimizing:
                        ckpt = tf.train.get_checkpoint_state(os.path.join(PARAMS.MODEL_URL, param_name))
                        test_saver.restore(sess, ckpt.model_checkpoint_path)
                    else:
                        test_saver.restore(sess, os.path.join(PARAMS.MODEL_URL, param_name, 'model.ckpt-{0}'.format(train_step)))

                    ### Write pb model
                    if PARAMS.IS_WRITE_PB:
                        constant_graph = tf.graph_util.convert_variables_to_constants(sess, sess.graph_def, ['test_pct'])
                        pb_model_name  = os.path.join(PARAMS.PB_URL, '{0}.pb'.format(datetime.datetime.now().strftime('%Y-%m-%d %H-%M-%S')))
                        with tf.gfile.FastGFile(pb_model_name, mode='wb') as f:
                            f.write(constant_graph.SerializeToString())

                    ### Get the number of test exammple.
                    num_test = rdata_handle.get_num_example(PARAMS.DATA_URL, PARAMS.FILE_PREFIXS[2])
                    
                    ### Initial the acc counter
                    acc_cnt  = 0.0

                    ### Test
                    for step in range(num_test):
                        ### Get test data and generate the feed_dict
                        run_test_data, run_test_images, run_test_labels = sess.run([test_data, test_images, test_labels])
                        feed_dict = {is_training:False, batch_images:run_test_images, batch_labels:run_test_labels}

                        ### Test the net
                        run_test_pct, run_test_acc = sess.run([test_pct, test_acc], feed_dict=feed_dict)

                        ### Counting the accuracy
                        acc_cnt += run_test_acc
                        
                        ### Output the error classfication
                        if run_test_acc == 0:
                            run_test_labelname = run_test_data['labelname'][0].decode()
                            run_test_filename  = run_test_data['filename'][0].decode()
                            logger.info('{0:05d}: Label = {1}, pct = {2}, Url = {3}'.format(step, run_test_labelname, 
                                        np.around(np.multiply(run_test_pct[0], 100), 2), run_test_filename))
                            logger.info('{0}![{1}]({2})'.format(' '*7, run_test_filename, 
                                        os.path.join(PARAMS.TXT_URL, run_test_filename)))

                    ### Calculate the whole test accuracy
                    all_test_acc = 'Test accuracy: {0:.4f} %'.format(acc_cnt / num_test * 100.0)
                    tb_writer.add_summary(sess.run(tf.summary.text('all_test_acc', tf.convert_to_tensor(all_test_acc))), 0)
                    logger.info(all_test_acc + '\n')

            ### Request stop the thread and wait it stop. 
            coord.request_stop()
            coord.join(threads)

    # ---------------------------------------------------------------- #
    # ---------------------------- Debug ----------------------------- #
    # ---------------------------------------------------------------- #
    if PARAMS.IS_DEBUG:
        img_handle.imshow(os.path.join(PARAMS.IMAGE_URL, b'OK\\B_0287_4_RD.bmp'.decode()))


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    ### Create logger
    log_url = '{0} {1} {2}.md'.format(PARAMS.NET1_NAME, PARAMS.NET2_NAME, datetime.datetime.now().strftime('%Y-%m-%d %H-%M-%S'))
    logger = log_handle.create_logger(logging.INFO, os.path.join(PARAMS.LOG_URL, log_url), True)

    ### Output informations
    log_handle.add_title(logger, 'Running at {0}'.format(datetime.datetime.now().strftime('%Y-%m-%d')))
    log_handle.add_infos(logger, PARAMS, data_info_line=[8, 9])

    ### Running
    if not PARAMS.IS_OPTIMIZING:
        run_net(PARAMS.NET_FUNC)
    else:
        ### Combine params
        for batch_size in PARAMS.BATCH_SIZE_GROUP:
            for learning_rate in PARAMS.LEARNING_RATE_GROUP:
                for dropout_keep_prob in PARAMS.DROPOUT_KEEP_PROB_GROUP:
                    ### Run net
                    run_net(PARAMS.NET_FUNC, True, PARAMS.TRAIN_STEP_GROUP, batch_size, learning_rate, dropout_keep_prob)


