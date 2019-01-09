# -*- coding: utf-8 -*-

''' log_handle.py

    Write logs using logging module. 

    Important:
        create_logger: Create a logger, then output a log using logger.info('Message'). 
        add_title    : Add    a title to log file.
        add_infos    : Add some infos to log file.

'''

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import os
import logging


def create_logger(level, fileurl, is_console):
    ''' Create a logger, then output a log using logger.info('Message'). 

        Args:
            level     : Logging level, like logging.DEBUG, logging.INFO, logging.WARNING, logging.ERROR, logging.CRITICAL.
            fileurl  : The name of log file.
            is_console: If output in the console at the same time. 

        Return:
            logger:

        Raise:
            ValueError('The level must be in [logging.DEBUG, logging.INFO, logging.WARNING, logging.ERROR, logging.CRITICAL]. ')
    '''
    ### Judging the input
    if level not in [logging.DEBUG, logging.INFO, logging.WARNING, logging.ERROR, logging.CRITICAL]:
        raise ValueError('The level must be in [logging.DEBUG, logging.INFO, logging.WARNING, logging.ERROR, logging.CRITICAL]. ')

    ### Create folder
    filedir, filename = os.path.split(fileurl)
    if not os.path.exists(filedir): 
        os.mkdir(filedir)

    ### Create a logger
    logger = logging.getLogger()
    logger.setLevel(level)
    
    ### Define the output format
    formatter = logging.Formatter('%(asctime)s - %(message)s')
    formatter.default_time_format = '%H:%M:%S'
    formatter.default_msec_format = '%s.%03d'

    ### Create a FileHandler for writing logs to a file
    fh = logging.FileHandler(fileurl)
    fh.setLevel(level)
    fh.setFormatter(formatter)
    logger.addHandler(fh)    

    ### Create a StreamHandler for writing logs to console
    if is_console:
        sh = logging.StreamHandler()
        sh.setLevel(level)
        sh.setFormatter(formatter)
        logger.addHandler(sh)  

    ### Return
    return logger


def add_title(logger, title, mode=1):
    ''' Add a title to log file. 

        Args:
            logger: The logger handler. 
            title : The title string. 
            mode  : 1 disp three lines, 2 disp left and right '-', 3 diap left '-'

        Return:

        Raise:
            raise ValueError('The title is too long. ')
            raise ValueError('The mode must be 1 or 2. ')
    '''
    ### Write mode == 1 or 2
    if mode == 1 or mode == 2:
        ### Define according the display in Typora
        max_num = 71
        if len(title) > max_num - 2:
            raise ValueError('The title is too long. ')

        ### Get the number of '-'
        add_num = max_num - len(title) - 2
        if add_num % 2 == 0:
            num1 = (int)(add_num / 2)
            num2 = num1
        else:
            num1 = (int)(add_num / 2)
            num2 = num1 + 1 

        ### Write title
        if mode == 1:
            logger.info('# {0} #'.format('-'*max_num))
            logger.info('# {0} {1} {2} #'.format('-'*num1, title, '-'*num2))
            logger.info('# {0} #'.format('-'*max_num))
        elif mode == 2:
            logger.info('# {0} {1} {2} #'.format('-'*num1, title, '-'*num2))
    ### Write mode == 3
    elif mode == 3:
        logger.info('# {0} {1}'.format('-'*16, title))
    ### Error mode
    else:
        raise ValueError('The mode must be 1 or 2. ')


def add_infos(logger, params, data_info_line):
    ''' Add some infos to log file. 

        Args:
            logger        : The logger handler. 
            params        : The global params. 
            data_info_line: [start_line, count_line]

        Return:

        Raise:
            
    '''

    ### Project infos
    logger.info('Project name: {0}'.format(params.PROJECT_NAME))
    logger.info('Net name: {0} {1}'.format(params.NET1_NAME, params.NET2_NAME))
    logger.info('')

    ### Data distribution
    data_info_url = os.path.join(params.DATA_URL, 'readme.txt')
    if os.path.exists(data_info_url):
        start_line = data_info_line[0]
        count_line = data_info_line[1]
        with open(data_info_url, 'r') as f:
            while start_line:
                tmp = f.readline()
                start_line = start_line - 1
            while count_line:
                logger.info(f.readline().replace('\n', ''))
                count_line = count_line - 1
            logger.info('')


# ---------------------------------------------------------------------------------------------------- #
# ----------------------------------------- Starting program ----------------------------------------- #
# ---------------------------------------------------------------------------------------------------- #
if __name__ == '__main__':
    pass

