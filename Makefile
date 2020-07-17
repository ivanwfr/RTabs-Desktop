###########################################################################################################
### C:/LOCAL/STORE/DEV/PROJECTS/GITHUB/Makefile ### SCRIPT_TAG (200717:22h:09) ############################
###########################################################################################################

include Make_GIT

:cd %:h|:update|:only|:terminal make links
links:  #-------------------- LINK ------------------------------------ TARGET
	(\
            mv -f RTabs_?????? RTabs;\
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

###########################################################################################################

