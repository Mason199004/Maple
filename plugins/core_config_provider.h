#ifndef CORE_CONFIG_PROVIDER_H
#define CORE_CONFIG_PROVIDER_H
#include "plugins.h"

extern U64 core_config_version;

extern LSTR core_config_name;

extern LSTR core_config_desc;

void init_core_config_provider(maple_plugin_procs proc_struct);

#endif //CORE_CONFIG_PROVIDER_H
