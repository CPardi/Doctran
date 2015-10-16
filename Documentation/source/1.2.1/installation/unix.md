#Linux and Mac OS X Installations
Installation on Unix based systems are distributed as a [makeself](https://github.com/megastep/makeself) package. To install place DoctranTrial.sh in the directory you would like to install Doctran within. Then, open a terminal within that directory and run 
    
    sh ./DoctranTrial.sh
    
and follow the instruction to install. The install package will check and warn you about any missing prerequisites. You will be asked if you want to add a symbolic link from Doctran to your /usr/bin/ folder. This will allow you to call Doctran from any folder. Confirming will require root access and you will be prompted to enter your root password.

##Prerequisites
The only prerequisite required currently is the Mono framework. This must be version 2.8 or above. Information on its installation can be found on Mono's [website](http://www.mono-project.com/download/)