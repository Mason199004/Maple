#include "plugins.h"

#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include "../logging/logging.h"
#include "core_config_provider.h"

maple_plugins plugins = {0};

BOOL maple_register_property(PROPERTY_DEF prop, PROPERTY_ACCESS external_access_level)
{

}

BOOL maple_write_property(PROPERTY* prop)
{

}

BOOL maple_read_property(LSTR property_name, PROPERTY* out_prop)
{

}

BOOL maple_get_property_info(LSTR property_name, PROPERTY_INFO* out_info)
{

}

BOOL maple_list_properties(LSTR_ARR* out_list)
{

}

void maple_plugin_log_error(LSTR str)
{
    //TODO
}

void maple_plugin_log_warning(LSTR str)
{
    //TODO
}

void maple_plugin_log_info(LSTR str)
{
    //TODO
}

BOOL maple_create_plugin(maple_plugin_info info, void (*init_proc)(maple_plugin_procs))
{
    if (plugins.num_plugins >= plugins.plugin_capacity)
    {
        void* temp = realloc(plugins.plugins, (plugins.plugin_capacity * 2) * sizeof(maple_plugin_KV));
        if (temp)
        {
            plugins.plugins = temp;
            memset(&plugins.plugins[plugins.num_plugins], 0, sizeof(maple_plugin_KV) * plugins.plugin_capacity);
            plugins.plugin_capacity *= 2;
        }
        else
        {
            maple_system_panic("CREATE_PLUGIN", "realloc failed!");
            //maybe just error here?
        }
    }

    if (!validate_plugin(&info))
    {
        return false;
    }

    plugins.plugins[plugins.num_plugins].info = info;
    plugins.plugins[plugins.num_plugins].data.init_proc = init_proc;
    plugins.plugins[plugins.num_plugins++].data.state = MP_UNLOADED;
    return true;
}

BOOL maple_load_plugin(LSTR plugin_name)
{
    for (int i = 0; i < plugins.num_plugins; ++i)
    {
        if (maple_lstrcmp(plugins.plugins[i].info.name, plugin_name))
        {
            if (plugins.plugins[i].data.state == MP_UNLOADED)
            {
                maple_plugin_procs procs;
                procs.struct_size = sizeof(procs);
                procs.maple_register_property = maple_register_property;
                procs.maple_write_property = maple_write_property;
                procs.maple_read_property = maple_read_property;
                procs.maple_get_property_info = maple_get_property_info;
                procs.maple_list_properties = maple_list_properties;
                procs.maple_plugin_log_error = maple_plugin_log_error;
                procs.maple_plugin_log_warning = maple_plugin_log_warning;
                procs.maple_plugin_log_info = maple_plugin_log_info;
                BOOL ready = plugins.plugins[i].data.init_proc(procs);

                if (ready)
                {
                    plugins.plugins[i].data.state = MP_READY;
                }
                else
                {
                    plugins.plugins[i].data.state = MP_LOADED;
                }
            }
        }
    }
}

BOOL validate_plugin(maple_plugin_info* info)
{
    if (maple_validate_printable_string(info->name))
    {
        if (maple_validate_printable_string(info->description))
        {
            for (int i = 0; i < plugins.num_plugins; ++i)
            {
                if (maple_lstrcmp(info->name, plugins.plugins[i].info.name))
                {
                    return false;
                }
            }
            return true;
        }
    }
    return false;
}


void maple_init_plugin_system()
{
    plugins.plugins = malloc(sizeof(maple_plugin_KV) * 256);
    plugins.plugin_capacity = 256;
    memset(plugins.plugins, 0, sizeof(maple_plugin_KV) * 256);

    maple_create_plugin((maple_plugin_info){.plugin_version = core_config_version, .name = core_config_name, .description = core_config_desc}, init_core_config_provider);
    maple_load_plugin(core_config_name);
}


