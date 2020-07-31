###############################################################################
### Makefile_TAG (200731:16h:58) ### DESKTOP (GITHUB) #########################
###############################################################################

### ANDROID:
###     $APROJECTS/GITHUB/Makefile
###     $APROJECTS/Makefile

### DESKTOP:
### ✔✔✔ $WPROJECTS/GITHUB/Makefile
###     $WPROJECTS/Makefile

ORIGIN = https://github.com/ivanwfr/RTabs-Desktop
include Make_GIT

:cd %:h|silent! make view_project_on_GITHUB
view_project_on_GITHUB: #{{{
	explorer $(ORIGIN)

#}}}

:cd %:h|up|only|set columns=999|vert terminal ++cols=150 make links
links: #{{{
	(\
	    mkdir RTabs;\
	    cd    RTabs;\
	    jar xvf ../RTabs_*.zip;\
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

:cd %:h|up|only|set columns=999|vert terminal ++cols=150 make clean
clean:                                                                         #{{{
	cd RTabs && make $@

# }}}

###############################################################################

# vim: noet ts=8 sw=8
