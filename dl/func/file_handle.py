# -*- coding: utf-8 -*-

''' file_handle.py

    Define some functions about file processing. 

    Important: 
        get_files: Get the file map [filenames, labelnames, labelvals] under the given url. 
    
    Useful: 
        get_sub_dir          : Get the urls and names of sub directories under the given url. 
        delete_files_randomly: Delete files randomly

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import os
import random

import numpy as np
import tensorflow as tf


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Get the file map ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def get_files(url, unique_labels, file_exts, seed=None):
    ''' Get the file map [filenames, labelnames, labelvals] under the given url. 

        Args:
            url          : The url of given directory. 
            unique_labels: The unique label names. 
            file_exts    : The extension of file, like ['bmp', 'jpg', 'png', 'tif', ...]
            seed         : The random seed in shuffled_index

        Return:
            [filenames, labelnames, labelvals]: 
                filenames : The filenames. 
                labelnames: Label names like 'Class1', 'Class2', 'Class3', ...
                labelvals : Label values like 1, 2, 3, ...
        Raises:
            
    '''
    ### Get all the files and assign [filenames, labelnames, labelvals]
    filenames  = []
    labelnames = []
    labelvals  = []
    labelval   = 0
    for labelname in unique_labels:
        # Only extract the files whose extension is in file_exts
        find_files = []
        for file_ext in file_exts:
            dir_url = os.path.join(url, labelname, '*.'+file_ext)
            try:
                find_file = tf.gfile.Glob(dir_url)
                find_files.extend(find_file)
            except:
                print('Except: {0} does not exist. '.format(dir_url))
        # Update the [filenames, labelnames, labelvals]
        filenames.extend(find_files)
        labelnames.extend([labelname] * len(find_files))
        labelvals.extend([labelval] * len(find_files))
        labelval += 1

    ### Shuffle the [filenames, labelnames, labelvals] in order to training
    # Using random seed to make sure the result of shuffled_index is the same
    shuffled_index = list(range(len(filenames)))
    random.seed(a=seed)                   
    random.shuffle(shuffled_index)

    filenames  = [filenames[i] for i in shuffled_index]
    labelnames = [labelnames[i] for i in shuffled_index]
    labelvals  = [labelvals[i] for i in shuffled_index]

    ### return
    print('Under folder {0}, get: {1} classes, {2} files. '.format(url, len(unique_labels), len(filenames)))
    return [filenames, labelnames, labelvals]


# ---------------------------------------------------------------------------------------------------- #
# --------------------------------- Functions about file processing ---------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
def get_sub_dir(url):
    ''' Get the urls and names of sub directories under the given url. 

        Args:
            url: The url of given directory. 

        Return:
            sub_urls: The urls of sub directories. 
            sub_names: The names of sub directories. 

        Raises:

    '''
    ### Get the urls of sub directories
    sub_urls = [x[0] for x in os.walk(url)]
    sub_urls.pop(0)

    ### Get the names of sub directories
    sub_names = []
    for sub_url in sub_urls:
        sub_names.append(os.path.split(sub_url)[1])

    ### return
    return sub_urls, sub_names


def delete_files_randomly(url, dlt_pct, file_exts, seed=None):
    ''' Delete files randomly. 

        Args: 
            url      : The url of given directory. 
            dlt_pct  : The percentage of deleted files. 
            file_exts: The extension of file, like ['bmp', 'jpg', 'png', 'tif', ...]
            seed     : The random seed in shuffled_index

        Return:

        Raises:

    '''
    ### Get all file names
    filenames = []
    for file_ext in file_exts:
        dir_url = os.path.join(url, '*.'+file_ext)
        try:
            find_file = tf.gfile.Glob(dir_url)
            filenames.extend(find_file)
        except:
            print('Except: {0} does not exist. '.format(dir_url))

    ### Shuffle the filenames in order to delete
    # Using random seed to make sure the result of shuffled_index is the same
    shuffled_index = list(range(len(filenames)))
    random.seed(a=seed)                   
    random.shuffle(shuffled_index)
    filenames = [filenames[i] for i in shuffled_index]

    ### Delete files
    if dlt_pct < 0:     dlt_pct = 0
    if dlt_pct > 100:   dlt_pct = 100
    for filename in filenames[0:(int)(len(filenames)*dlt_pct/100.0)]:
        os.remove(filename)


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    delete_files_randomly(r'D:\00 Run\00 Deep Learning\06 OPW\Images\Images_V\OK', 90, file_exts=['bmp', 'jpg', 'png', 'tif'], seed=1012)



