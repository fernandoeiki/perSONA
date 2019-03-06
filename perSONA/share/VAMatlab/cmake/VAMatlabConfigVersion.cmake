set( VAMATLAB_VERSION_TYPE    "" )
set( VAMATLAB_VERSION_NAME    "" )
set( VAMATLAB_VERSION_MAJOR     )
set( VAMATLAB_VERSION_MINOR     )
set( VAMATLAB_VERSION_PATCH     )
set( VAMATLAB_VERSION_TWEAK     )
set( VAMATLAB_VERSION_HWARCH  "win32-x64.vc12" )

set( VAMATLAB_VERSION         "" )
set( VAMATLAB_VERSION_EXT     "" )
	
# we store the root dir in order to check if a new installation should delete this one
if( "${PACKAGE_FIND_VERSION}" STREQUAL "" AND "${PACKAGE_FIND_VERSION_EXT}" STREQUAL "" )
	# no version given - just accept
	set( PACKAGE_VERSION_EXACT FALSE )
	set( PACKAGE_VERSION_COMPATIBLE TRUE )
	set( PACKAGE_VERSION_UNSUITABLE FALSE )

else()
	# a special macro for loading the the custom versioned file, since luckily CMakes versioning can
	# handle just nothing but versions like 2.3.11.42 - absolutely perfect for development versions and branching
	# we'd prefer to have the option to use find_package with a more elaborate versioning, including HEAD/TAG/BRANCHNAMES
	# and version ranges
	# if the versionmatches, the flag is NOT ALTERED! but only set to FALSE if it fails
	macro( _local_check_valid_subversion INPUT_SUBVERSION OWN_SUBVERSION ALLOW_COMPATIBLE_ON_LOWER )
		if( "${INPUT_SUBVERSION}" STREQUAL "" OR "${OWN_SUBVERSION}" STREQUAL "" )
			# fine for us, just accept
		elseif( "${INPUT_SUBVERSION}" STREQUAL "" OR "${OWN_SUBVERSION}" STREQUAL "" )
			# exact match
		elseif( ALLOW_COMPATIBLE_ON_LOWER AND ${INPUT_SUBVERSION} VERSION_LESS ${OWN_SUBVERSION} )
			# compatible match
			set( PACKAGE_VERSION_EXACT FALSE )
		else()

			#check if it is NUMBER+ - anything equal or bigger to number
			string( REGEX MATCH "([0-9]+)\\+$" _STRING_IS_MIN ${INPUT_SUBVERSION} )
			if( _STRING_IS_MIN )
				if( ${OWN_SUBVERSION} LESS CMAKE_MATCH_1 )
					set( PACKAGE_VERSION_EXACT FALSE )
					set( PACKAGE_VERSION_COMPATIBLE FALSE )
				endif( ${OWN_SUBVERSION} LESS CMAKE_MATCH_1 )
			else( _STRING_IS_MIN )

				#check if it is NUMBER-NUMBER - an explicit range
				string( REGEX MATCH "([0-9]+)\\-([0-9]+)$" _STRING_IS_RANGE ${INPUT_SUBVERSION} )
				if( _STRING_IS_RANGE )
					if( NOT ( ${OWN_SUBVERSION} EQUAL CMAKE_MATCH_1 ) OR ( ${OWN_SUBVERSION} GREATER CMAKE_MATCH_1 ) )
						set( PACKAGE_VERSION_EXACT FALSE )
						set( PACKAGE_VERSION_COMPATIBLE FALSE )
					elseif( NOT ( ${OWN_SUBVERSION} EQUAL CMAKE_MATCH_1 ) OR ( ${OWN_SUBVERSION} GREATER CMAKE_MATCH_1 ) )
						if( ( ${OWN_SUBVERSION} EQUAL CMAKE_MATCH_2 ) OR ( ${OWN_SUBVERSION} LESS CMAKE_MATCH_2 ) )
							set( PACKAGE_VERSION_EXACT FALSE )
							set( PACKAGE_VERSION_COMPATIBLE FALSE )
						endif( ( ${OWN_SUBVERSION} EQUAL CMAKE_MATCH_2 ) OR ( ${OWN_SUBVERSION} LESS CMAKE_MATCH_2 ) )
					endif( NOT ( ${OWN_SUBVERSION} EQUAL CMAKE_MATCH_1 ) OR ( ${OWN_SUBVERSION} GREATER CMAKE_MATCH_1 ) )
				else( _STRING_IS_RANGE )
					# check if it is just a numer
					if( NOT ${INPUT_SUBVERSION} EQUAL ${OWN_SUBVERSION} )
						set( PACKAGE_VERSION_EXACT FALSE )
						if( NOT ALLOW_COMPATIBLE_ON_LOWER OR ${INPUT_SUBVERSION} GREATER ${OWN_SUBVERSION} )
							set( PACKAGE_VERSION_COMPATIBLE FALSE )
						endif( NOT ALLOW_COMPATIBLE_ON_LOWER OR ${INPUT_SUBVERSION} GREATER ${OWN_SUBVERSION} )
					endif( NOT ${INPUT_SUBVERSION} EQUAL ${OWN_SUBVERSION} )
				endif( _STRING_IS_RANGE )

			endif( _STRING_IS_MIN  )
		endif( "${INPUT_SUBVERSION}" STREQUAL "" OR  "${OWN_SUBVERSION}" STREQUAL "" )
	endmacro( _local_check_valid_subversion )

	macro( _local_extract_part _TARGET _ENTRY _SEPARATOR  )
		set( ${_TARGET} )
		if( _REMAINING_VERSION )
			string( REGEX MATCH "^(${_ENTRY})${_SEPARATOR}(.*)$" _MATCH_SUCCESS ${_REMAINING_VERSION} )
			if( _MATCH_SUCCESS )
				# we found a textual start -> type
				set( ${_TARGET} ${CMAKE_MATCH_1} )
				set( _REMAINING_VERSION ${CMAKE_MATCH_2} )
			else( _MATCH_SUCCESS )
				string( REGEX MATCH "^(${_ENTRY})$" _MATCH2_SUCCESS ${_REMAINING_VERSION} )
				if( _MATCH2_SUCCESS )
					# we found a textual start -> type
					set( ${_TARGET} ${CMAKE_MATCH_1} )
					set( _REMAINING_VERSION ${CMAKE_MATCH_2} )
				endif( _MATCH2_SUCCESS )
			endif( _MATCH_SUCCESS )
		endif( _REMAINING_VERSION )
	endmacro( _local_extract_part _TARGET _ENTRY _SEPARATOR )

	if( NOT PACKAGE_FIND_VERSION_EXT )
		set( PACKAGE_VERSION "${VAMATLAB_VERSION}" )

		# the cmake native versioning system: just allows an exact match with MAJOR.MINOR.PATCH - just works for releases!
		if( VAMATLAB_VERSION_TYPE STREQUAL "RELEASE" )
			set( PACKAGE_VERSION_EXACT TRUE )
			set( PACKAGE_VERSION_COMPATIBLE TRUE )
			set( PACKAGE_VERSION_UNSUITABLE FALSE )

			if( NOT PACKAGE_FIND_VERSION_MAJOR VERSION_EQUAL VAMATLAB_VERSION_MAJOR )
				set( PACKAGE_VERSION_EXACT FALSE )
				set( PACKAGE_VERSION_COMPATIBLE FALSE )
			endif( NOT PACKAGE_FIND_VERSION_MAJOR VERSION_EQUAL VAMATLAB_VERSION_MAJOR )

			if( PACKAGE_FIND_VERSION_COUNT GREATER 1 )
				if( NOT PACKAGE_FIND_VERSION_MINOR VERSION_EQUAL VAMATLAB_VERSION_MINOR )
					set( PACKAGE_VERSION_EXACT FALSE )
					set( PACKAGE_VERSION_COMPATIBLE FALSE )
				endif( NOT PACKAGE_FIND_VERSION_MINOR VERSION_EQUAL VAMATLAB_VERSION_MINOR )
			endif( PACKAGE_FIND_VERSION_COUNT GREATER 1 )

			if( PACKAGE_FIND_VERSION_COUNT GREATER 2 )
				if( NOT PACKAGE_FIND_VERSION_PATCH VERSION_EQUAL VAMATLAB_VERSION_PATCH )
					set( PACKAGE_VERSION_EXACT FALSE )
					if( NOT PACKAGE_FIND_VERSION_PATCH VERSION_LESS VAMATLAB_VERSION_PATCH )
						set( PACKAGE_VERSION_COMPATIBLE FALSE )
					endif( NOT PACKAGE_FIND_VERSION_PATCH VERSION_LESS VAMATLAB_VERSION_PATCH )
				endif( NOT PACKAGE_FIND_VERSION_PATCH VERSION_EQUAL VAMATLAB_VERSION_PATCH )
			endif( PACKAGE_FIND_VERSION_COUNT GREATER 2 )

			if( PACKAGE_FIND_VERSION_COUNT GREATER 3 )
				if( NOT PACKAGE_FIND_VERSION_TWEAK VERSION_EQUAL VAMATLAB_VERSION_TWEAK )
					set( PACKAGE_VERSION_EXACT FALSE )
					if( NOT PACKAGE_FIND_VERSION_TWEAK VERSION_LESS VAMATLAB_VERSION_TWEAK )
						set( PACKAGE_VERSION_COMPATIBLE FALSE )
					endif( NOT PACKAGE_FIND_VERSION_TWEAK VERSION_LESS VAMATLAB_VERSION_TWEAK )
				endif( NOT PACKAGE_FIND_VERSION_TWEAK VERSION_EQUAL VAMATLAB_VERSION_TWEAK )
			endif( PACKAGE_FIND_VERSION_COUNT GREATER 3 )

		else( VAMATLAB_VERSION_TYPE STREQUAL "RELEASE" )

			set( PACKAGE_VERSION_EXACT FALSE )
			set( PACKAGE_VERSION_COMPATIBLE FALSE )
			set( PACKAGE_VERSION_UNSUITABLE TRUE )

		endif( VAMATLAB_VERSION_TYPE STREQUAL "RELEASE" )

	else( NOT PACKAGE_FIND_VERSION_EXT )
		set( PACKAGE_VERSION "${VAMATLAB_VERSION_EXT}" )

		# first, we decompose the input
		set( _REMAINING_VERSION ${PACKAGE_FIND_VERSION_EXT} )

		_local_extract_part( _IN_TYPE  "[a-zA-Z]+" "_" )

		if( NOT _IN_TYPE STREQUAL "HEAD"
			AND NOT _IN_TYPE STREQUAL "RELEASE"
			AND NOT _IN_TYPE STREQUAL "BRANCH"
			AND NOT _IN_TYPE STREQUAL "TRUNK" )
			set( _IN_TYPE )
			set( _REMAINING_VERSION ${PACKAGE_FIND_VERSION_EXT} )
		endif( NOT _IN_TYPE STREQUAL "HEAD"
			AND NOT _IN_TYPE STREQUAL "RELEASE"
			AND NOT _IN_TYPE STREQUAL "BRANCH"
			AND NOT _IN_TYPE STREQUAL "TRUNK" )

		_local_extract_part( _IN_NAME  "[a-zA-Z][^\\\\-]+" "\\\\-" )
		_local_extract_part( _IN_MAJOR "[0-9\\\\+\\\\-]+" "\\\\." )
		_local_extract_part( _IN_MINOR "[0-9\\\\+\\\\-]+" "\\\\." )
		_local_extract_part( _IN_PATCH "[0-9\\\\+\\\\-]+" "\\\\." )
		_local_extract_part( _IN_TWEAK "[0-9\\\\+\\\\-]+" "$" )

		#we assume match, and only change this if we find non-matches
		set( PACKAGE_VERSION_EXACT TRUE )
		set( PACKAGE_VERSION_COMPATIBLE TRUE )
		set( PACKAGE_VERSION_UNSUITABLE FALSE )

		#if there is just one entry, it's the name
		if( _IN_TYPE AND NOT _IN_NAME AND NOT _IN_MAJOR )
			set( _IN_NAME ${_IN_TYPE} )
			set( _IN_TYPE "" )
		endif( _IN_TYPE AND NOT _IN_NAME AND NOT _IN_MAJOR )

		if( NOT _IN_TYPE OR _IN_TYPE STREQUAL VAMATLAB_VERSION_TYPE )
			# in case of releases, we don't need a name
			if( NOT _IN_NAME AND VAMATLAB_VERSION_TYPE STREQUAL "RELEASE" )
				set( _IN_NAME ${VAMATLAB_VERSION_NAME} )
			endif( NOT _IN_NAME AND VAMATLAB_VERSION_TYPE STREQUAL "RELEASE" )

			# in case of head, either type or name has to be head
			if( _IN_TYPE STREQUAL "HEAD" AND NOT _IN_NAME )
				set( _IN_NAME "HEAD" )
			endif( _IN_TYPE STREQUAL "HEAD" AND NOT _IN_NAME )

			#check name
			if( NOT _IN_NAME STREQUAL VAMATLAB_VERSION_NAME )
				set( PACKAGE_VERSION_EXACT FALSE )
				set( PACKAGE_VERSION_COMPATIBLE FALSE )
			else( NOT _IN_NAME STREQUAL VAMATLAB_VERSION_NAME )
				#check versions - these automatically adjust PACKAGE_VERSION_EXACT and PACKAGE_VERSION_COMPATIBLE
				_local_check_valid_subversion( "${_IN_MAJOR}" "${VAMATLAB_VERSION_MAJOR}" FALSE )
				_local_check_valid_subversion( "${_IN_MINOR}" "${VAMATLAB_VERSION_MINOR}" FALSE )
				_local_check_valid_subversion( "${_IN_PATCH}" "${VAMATLAB_VERSION_PATCH}" TRUE )
				_local_check_valid_subversion( "${_IN_TWEAK}" "${VAMATLAB_VERSION_TWEAK}" TRUE )
			endif( NOT _IN_NAME STREQUAL VAMATLAB_VERSION_NAME )

		else( NOT _IN_TYPE OR _IN_TYPE STREQUAL VAMATLAB_VERSION_TYPE )
			set( PACKAGE_VERSION_EXACT FALSE )
			set( PACKAGE_VERSION_COMPATIBLE FALSE )
		endif( NOT _IN_TYPE OR _IN_TYPE STREQUAL VAMATLAB_VERSION_TYPE )

	endif( NOT PACKAGE_FIND_VERSION_EXT )

endif( "${PACKAGE_FIND_VERSION}" STREQUAL "" AND "${PACKAGE_FIND_VERSION_EXT}" STREQUAL "" )

# finally: verify that the HWARCH is correct (if the var exists)
if( VISTA_HWARCH AND NOT "${VISTA_HWARCH}" STREQUAL "${VAMATLAB_VERSION_HWARCH}" )
	set( PACKAGE_VERSION_EXACT FALSE )
	set( PACKAGE_VERSION_COMPATIBLE FALSE )
	set( PACKAGE_VERSION_UNSUITABLE TRUE )
endif( VISTA_HWARCH AND NOT "${VISTA_HWARCH}" STREQUAL "${VAMATLAB_VERSION_HWARCH}" )


