#include "core_config_provider.h"

U64 core_config_version = MAKE_VERNUM(0, 0, 1, 0);

LSTR core_config_name = MAKE_LSTR("CoreCFG");

LSTR core_config_desc = MAKE_LSTR("Provides a simple text based config format");

BOOL core_config_read_file(const char* path)
{

}

BOOL core_config_write_file(const char* path)
{

}

maple_plugin_procs procs = {0};
void init_core_config_provider(maple_plugin_procs proc_struct)
{
	if (proc_struct.struct_size != sizeof(maple_plugin_procs))
	{
		//wat
		maple_plugin_log_error(MAKE_LSTR("Mismatch plugin procs somehow"));
		return;
	}
	procs = proc_struct;

	LSTR readName = MAKE_LSTR("MI_ProjectFormatProvider_CORE_ReadFile");
	LSTR writeName = MAKE_LSTR("MI_ProjectFormatProvider_CORE_WriteFile");

	BOOL registered_read = maple_register_property((PROPERTY_DEF){.name = readName, .type = M_PTR}, M_NONE);
	BOOL registered_write = maple_register_property((PROPERTY_DEF){.name = writeName, .type = M_PTR}, M_NONE);

	if (registered_read == false || registered_write == false)
	{
		maple_plugin_log_error(MAKE_LSTR("Failed to register properties"));
		return;
	}

	BOOL set_read = maple_write_property((PROPERTY){.name = readName, .value = core_config_read_file});
	BOOL set_write = maple_write_property((PROPERTY){.name = writeName, .value = core_config_write_file});

	if (set_read == false || set_write == false)
	{
		maple_plugin_log_error(MAKE_LSTR("Failed to set properties"));
		return;
	}

}