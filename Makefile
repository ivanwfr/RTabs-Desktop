###############################################################################
<<<<<<< HEAD
### https://github.com/ivanwfr/RTabs-Desktop ##### Makefile_TAG (200722:18h:33)
=======
### https://github.com/ivanwfr/RTabs-Desktop ##### Makefile_TAG (200722:17h:52)
>>>>>>> c65e7173283b098c47bd40314dc9e197f5a87a39
###############################################################################
# VARS {{{
 ORIGIN = https://github.com/ivanwfr/RTabs-Desktop
    JAR = jar.exe
#}}}

include Make_GIT

:cd %:h|make view_project
view_project: #{{{
	explorer $(ORIGIN)

#}}}

:cd %:h|up|only|set columns=999|vert terminal ++cols=150 make links
links: #{{{
	(\
	    mkdir RTabs;\
<<<<<<< HEAD
=======
	    \
>>>>>>> c65e7173283b098c47bd40314dc9e197f5a87a39
	    cd    RTabs;\
	    $(JAR) xvf ../RTabs_*.zip;\
	    \
	    :---------------- LINK ------------------------------------ TARGET;\
	    cmd /c mklink /J 'RTabsDesigner\Util'                       'Util';\
	    cmd /c mklink /J 'RTabsServer\Util'                         'Util';\
	    \
	    cmd /c mklink /H 'RTabsServer\src\MainForm.cs'              'RTabs\src\MainForm.cs';\
	    cmd /c mklink /H 'RTabsServer\src\MainForm.Designer.cs'     'RTabs\src\MainForm.Designer.cs';\
	    \
	    cmd /c mklink /H 'RTabsDesigner\src\MainForm.cs'            'RTabs\src\MainForm.cs';\
	    cmd /c mklink /H 'RTabsDesigner\src\MainForm.Designer.cs'   'RTabs\src\MainForm.Designer.cs';\
	    )
	#}}}

###############################################################################

# vim: noet ts=8 sw=8
