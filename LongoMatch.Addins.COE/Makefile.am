ASSEMBLY = LongoMatch.Addins.COE
TARGET = library
INSTALL_DIR = $(pkglibdir)/plugins
EPPlus_DLL = EPPlus/EPPlus/bin/Debug/EPPlus.dll

LINK = $(REF_DEP_LONGOMATCH_PLUGINS) \
       $(LINK_SYSTEM_DRAWING) \
       -r:$(EPPlus_DLL)

SOURCES = \
	Assembly.cs\
	ExcelExporter.cs\
	GameUnitsEnabler.cs\
	GameUnitsStatsSheet.cs\
	GameUnitsStatsSheetFull.cs\
	ProjectStatsSheet.cs\
	TeamStatsSheet.cs\
	TimelineSheet.cs

RESOURCES =

EPPlus:
	git clone git@github.com:ylatuya/EPPlus.git

$(EPPlus_DLL): EPPlus
	xbuild  EPPlus/EPPlus/EPPlus.csproj

include $(top_srcdir)/build/build.mk

$(ASSEMBLY_FILE): $(EPPlus_DLL)

module_SCRIPTS += $(EPPlus_DLL)
