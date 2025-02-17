#include "core.h"

#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>
#include "../logging/logging.h"

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
        // ReSharper disable once CppDFAMemoryLeak
        config_dir = malloc(strlen(xdg_config_home) + strlen("/maple") + 1);
        sprintf(config_dir, "%s/maple", xdg_config_home);
    }
    else
    {
        // ReSharper disable once CppDFAMemoryLeak
        config_dir = malloc(strlen(home_dir) + strlen("/.config/maple") + 1);
        sprintf(config_dir, "%s/.config/maple", home_dir);
    }
#endif

    return config_dir;
}

void maple_init()
{
    // ReSharper disable once CppDFAMemoryLeak
    char* config_dir = maple_get_config_dir();
    struct stat config = {0};
    char* plugin_dir = malloc(strlen(config_dir) + strlen("/plugins") + 1);

    if (!stat(config_dir, &config))
    {
        if (S_ISDIR(config.st_mode))
        {
            sprintf(plugin_dir, "%s/plugins", config_dir);
            config = {0};
            if (!stat(plugin_dir, &config))
            {
                if (S_ISDIR(config.st_mode))
                {
                    //since the majority of the applications behavior will originate from core plugins (statically linked)
                    //all we need to do here is to ensure the config and plugins dirs actually exist for when we later
                    //load any additional plugins and when core plugins attempt to further bootstrap the app

                    //statically linked core plugins is just to avoid needing to have an install process, maybe one
                    //day maple will have a package manager that can install/update them
                }
                else
                {
                    mkdir(plugin_dir, 0755);
                }
            }
            else
            {
                maple_system_panic("CORE_INIT", "Failed to stat plugin dir!");
            }
        }
        else
        {
            mkdir(config_dir, 0755);
        }
    }
    else
    {
        //stat failed, bail
        maple_system_panic("CORE_INIT", "Failed to stat config dir!");
    }


    free(config_dir);
    free(plugin_dir);
}
