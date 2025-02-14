#include "plugins.h"

#include <stdlib.h>
#include <string.h>
#include <stdio.h>

MAPLE_PLUGIN_INFO* Plugins;
I32 PluginCount;

BOOL validate_plugin(MAPLE_PLUGIN_INFO* info)
{
    if (maple_validate_printable_string(info->name))
    {
        if (maple_validate_printable_string(info->description))
        {
            for (int i = 0; i < PluginCount; ++i)
            {
                if (maple_lstrcmp(info->name, Plugins[i].name))
                {
                    return false;
                }
            }
            return true;
        }
    }
    return false;
}

char* maple_get_config_dir()
{
    char *home_dir = NULL;
    char *config_dir = NULL;

#ifdef _WIN32
    char *appdata = getenv("APPDATA");
    if (appdata)
    {
        config_dir = malloc(strlen(appdata) + strlen("\\maple") + 1);
        sprintf(config_dir, "%s\\maple", appdata);
    }
    else
    {
        home_dir = getenv("USERPROFILE");

        config_dir = malloc(strlen(home_dir) + strlen("\\AppData\\Roaming\\maple") + 1);
        sprintf(config_dir, "%s\\AppData\\Roaming\\maple", home_dir);
    }

#else
    home_dir = getenv("HOME");
    char *xdg_config_home = getenv("XDG_CONFIG_HOME");

    if (xdg_config_home)
    {
        config_dir = malloc(strlen(xdg_config_home) + strlen("/maple") + 1);
        sprintf(config_dir, "%s/maple", xdg_config_home);
    }
    else
    {
        config_dir = malloc(strlen(home_dir) + strlen("/.config/maple") + 1);
        sprintf(config_dir, "%s/.config/maple", home_dir);
    }
#endif

    return config_dir;
}


void maple_init_plugin_system()
{
    Plugins = malloc(sizeof(MAPLE_PLUGIN_INFO) * 256);
    memset(Plugins, 0, sizeof(MAPLE_PLUGIN_INFO) * 256);
    PluginCount = 0;
}


