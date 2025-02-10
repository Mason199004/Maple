#include "plugins.h"
#include <string.h>

MAPLE_PLUGIN_INFO* Plugins[256];
void (*plugin_callbacks[256])(maple_plugin_start_info*);
I32 PluginCount;

BOOL validate_plugin(MAPLE_PLUGIN_INFO* info)
{
    if (maple_validate_printable_string(info->name))
    {
        if (maple_validate_printable_string(info->description))
        {
            for (int i = 0; i < PluginCount; ++i)
            {
                if (maple_lstrcmp(info->name, Plugins[i]->name))
                {
                    return false;
                }
            }
            return true;
        }
    }
    return false;
}


void maple_plugin_log_error();

void maple_plugin_log_warning();

void maple_plugin_log_info();

void maple_init_plugin_system()
{
    memset(Plugins, 0, sizeof(MAPLE_PLUGIN_INFO*) * 256);
    memset(plugin_callbacks, 0, sizeof(void*) * 256);
    PluginCount = 0;
}


