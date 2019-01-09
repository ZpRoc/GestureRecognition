# -*- coding: utf-8 -*-

''' img_handle.py

    Define some functions about image processing

    Important: 
        img_pproc: Image preprocessing before training or test.
        imshow   : Show image.

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import os
import random
import sys

import numpy as np
import tensorflow as tf
import PIL.Image as ImgProc
import matplotlib.pyplot as plt

# ---------------------------------------------------------------------------------------------------- #
# --------------------------------------- Image Pre-processing --------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def img_pproc(img, adjust_params=None, random_params=None):
    ''' Image preprocessing before training or test. 

        Args:
            img                   : The image (Tensor), using tf.image.
            is_img_std            : [Enabled]
            adjust_contrast       : [Enabled, factor]
            adjust_brightness     : [Enabled, delta]
            random_contrast       : [Enabled, lower, upper]
            random_brightness     : [Enabled, max_delta]
            random_flip_left_right: [Enabled]
            random_flip_up_down   : [Enabled]

        Return:
            img: 

        Raises:
            
        Notes:
            tf.image.adjust_contrast(images, contrast_factor)
                images         : Images to adjust. At least 3-D.
                contrast_factor: A float multiplier for adjusting contrast.

            tf.image.adjust_brightness(image, delta)
                image: A tensor.
                delta: A scalar. Amount to add to the pixel values. (in the range [0,1))

            tf.image.random_contrast(image, lower, upper, seed=None)
                image: An image tensor with 3 or more dimensions.
                lower: float. Lower bound for the random contrast factor. (lower > 0)
                upper: float. Upper bound for the random contrast factor. (upper > lower)
                Notes: Equivalent to adjust_contrast() but uses a contrast_factor randomly picked in the interval [lower, upper].

            tf.image.random_brightness(image, max_delta, seed=None)
                image    : An image.
                max_delta: float, must be non-negative.
                Notes    : Equivalent to adjust_brightness() using a delta randomly picked in the interval [-max_delta, max_delta).

            tf.image.random_flip_left_right(image, seed=None)
                image: 4-D Tensor of shape [batch, height, width, channels] or 3-D Tensor of shape [height, width, channels].

            tf.image.random_flip_up_down(image, seed=None)
                image: 4-D Tensor of shape [batch, height, width, channels] or 3-D Tensor of shape [height, width, channels].
    '''
    # -------------------- Get params -------------------- #
    ### Get the adjust image preprocessing params
    if adjust_params==None:
        is_img_std             = [False]
        adjust_contrast        = [False, 1.0]
        adjust_brightness      = [False, 0.0]
    else:
        is_img_std             = adjust_params[0]
        adjust_contrast        = adjust_params[1]
        adjust_brightness      = adjust_params[2]

    ### Get the random image preprocessing params
    if random_params==None:
        random_contrast        = [False, 1.0, 1.0]
        random_brightness      = [False, 0.0]
        random_flip_left_right = [False]
        random_flip_up_down    = [False]
    else:
        random_contrast        = random_params[0]
        random_brightness      = random_params[1]
        random_flip_left_right = random_params[2]
        random_flip_up_down    = random_params[3]

    # -------------------- Convert image -------------------- #
    ### Convert to float
    img = tf.cast(img, tf.float32)

    # -------------------- Adjust image -------------------- #
    ### adjust_contrast       : [Enabled, factor]
    if adjust_contrast[0]:
        img = tf.image.adjust_contrast(img, adjust_contrast[1])

    ### adjust_brightness     : [Enabled, delta]
    if adjust_brightness[0]:
        img = tf.image.adjust_brightness(img, adjust_brightness[1])

    # -------------------- Random image -------------------- #
    ### random_contrast       : [Enabled, lower, upper]
    if random_contrast[0]:
        img = tf.image.random_contrast(img, random_contrast[1], random_contrast[2])

    ### random_brightness     : [Enabled, max_delta]
    if random_brightness[0]:
        img = tf.image.random_brightness(img, random_brightness[1])

    ### random_flip_left_right: [Enabled]
    if random_flip_left_right[0]:
        img = tf.image.random_flip_left_right(img)

    ### random_flip_up_down   : [Enabled]
    if random_flip_up_down[0]:
        img = tf.image.random_flip_up_down(img)

    # -------------------- Std image -------------------- #
    ### Standardization image
    if is_img_std[0]:
        img = tf.image.per_image_standardization(img)

    return img


# ---------------------------------------------------------------------------------------------------- #
# -------------------------------------------- Image Disp -------------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def imshow(img_url):
    ''' Show image. 

        Args:
            img_url: The url of image. 

        Return:

        Raises:

    '''
    ### 
    if isinstance(img_url, bytes):
        img_url = img_url.decode()

    ### Read image
    img = ImgProc.open(img_url)

    ### Show image
    plt.imshow(img)
    plt.show()


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    pass



