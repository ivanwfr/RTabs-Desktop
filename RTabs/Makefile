###########################################################################################################
### C:/LOCAL/STORE/DEV/PROJECTS/RTabs/Makefile ### SCRIPT_TAG (200717:17h:09) #############################
###########################################################################################################
include Make_GIT
 TOP		= $(LOCAL)/STORE/DEV/PROJECTS
 APPDIR		= RTabs
 MODULES	= RTabs RTabsDesigner RTabsServer Util .
 FIND		= find.exe
 LS		= ls.exe
 JAR		= jar.exe
 RAR		= rar.exe
 RAS            = -av- -c- -cfg- -ed -idp -inul -tl
 FILE_FILTER    = sed -e '/^\.\/[^\/]*$$/d' -e 's/^\.\///' -e '/Images/d' -e '/IMAGES/d' -e '/debug/d' -e '/Debug/d' -e '/DEBUG/d'

default: clean arch tags

:cd %:h|:update|:only|:terminal make links
links:  #-------------------- LINK ------------------------------------ TARGET
	(\
            cd RTabs;\
            cmd /c mklink /J 'RTabsDesigner\Util'                       'Util';\
            cmd /c mklink /J 'RTabsServer\Util'                         'Util';\
            \
            cmd /c mklink /H 'RTabsServer\src\MainForm.cs'              'RTabs\src\MainForm.cs';\
            cmd /c mklink /H 'RTabsServer\src\MainForm.Designer.cs'     'RTabs\src\MainForm.Designer.cs';\
            \
            cmd /c mklink /H 'RTabsDesigner\src\MainForm.cs'            'RTabs\src\MainForm.cs';\
            cmd /c mklink /H 'RTabsDesigner\src\MainForm.Designer.cs'   'RTabs\src\MainForm.Designer.cs';\
	)

arch:                                                                     #{{{
	@(\
            echo "cd TOP [$(TOP)]";\
	    cd $(TOP);\
            pwd;\
            echo "APPDIR=[$(APPDIR)]";\
	    FILES=`$(FIND) $(APPDIR) -type f\
	    | sed                       \
            -e "s/ /?/g"                \
            \
	    -e "/\/LINKS/d"		\
	    -e "/\/bin.*/d"	        \
	    -e "/\\obj.*/d"	        \
	    -e "/tags/d"		\
            \
	    -e "/\.application/d"	\
	    -e "/\.dll/d"		\
	    -e "/\.exe*/d"		\
	    -e "/\.git*/d"		\
	    -e "/\.lnk/d"		\
	    -e "/\.rar*/d"		\
	    -e "/\.orig/d"		\
	    `;\
	    DATE=`date '+%y%m%d'`;\
	    ARCHIVE="$(APPDIR)_$$DATE.rar";\
	    echo $(RAR)   a $(RAS) ARCHIVES/$$ARCHIVE $$FILES;\
	         $(RAR)   a $(RAS) ARCHIVES/$$ARCHIVE $$FILES;\
	    $(LS)       -lt ARCHIVES/*.rar | head;\
	    $(RAR)       l  ARCHIVES/$$ARCHIVE;\
	)#

# }}}
clean:                                                                    #{{{
	rm -rf .vs */.vs obj */obj/
	cd $(TOP) && rm -rf tags */tags */*/tags

# }}}
help:                                                                     #{{{
	$(ANT) -projecthelp

# }}}
.PHONY: tags                                                              #{{{
tags:
	cd $(TOP)/$(APPDIR) && ctags -R --links=no RTabs *Designer *Server Util && wc -l tags

#}}}
wc: # {{{
	(\
	for i in $(MODULES); do\
	    echo $$i '{{{ ';\
	    FILES=`$(FIND) $$i -name '*.cs'\
	    | $(FILE_FILTER)\
	    | sort`;\
	    wc $$FILES;\
	    echo '}}} ';\
	    echo;\
	done;\
	)

# }}}
touch:  # sync link last modified time                                    #{{{
	ls    -l          RTabs/src/MainForm.cs     RTabsDesigner/src/MainForm.cs RTabsServer/src/MainForm.cs
	touch -c -h -m -r RTabs/src/MainForm.cs     RTabsDesigner/src/MainForm.cs RTabsServer/src/MainForm.cs 
	ls    -l          RTabs/src/MainForm.cs     RTabsDesigner/src/MainForm.cs RTabsServer/src/MainForm.cs

# }}}
# {{{
# ---------------------------------------------------------------------
# }!!rar -?
# 
# ---------------------------------------------------------------------
# $(RAR)   a $(RAS) ARCHIVES/$$ARCHIVE $$FILES;\
# $(RAR)   lv       ARCHIVES/$$ARCHIVE;\
# ---------------------------------------------------------------------
# Usage:     rar <command> -<switch 1> -<switch N> <archive> <files...>
# <Commands>
#  v[t,b]        Verbosely list archive [technical,bare]
# <Switches>
#   av-           Disable authenticity verification check
#   c-            Disable comments show
#   cfg-          Disable read configuration
#   ed            Do not add empty directories
#   idp           Disable percentage display
#   inul          Disable all messages
#   tl            Set archive time to latest file
# ---------------------------------------------------------------------
# }}}
