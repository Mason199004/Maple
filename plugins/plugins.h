#include "../MapleTypes.h"
#include "../Project/MapleProj.h"
#ifndef MAPLE_PLUGINS_H
#define MAPLE_PLUGINS_H

typedef struct
{
    U64 plugin_version; //x.x.x.x each value is 2 bytes
    LSTR* name;
    LSTR* description;
    //possibly permissions/intents
} MAPLE_PLUGIN_INFO;

typedef struct
{

} maple_plugin_start_info;

BOOL maple_register_plugin(MAPLE_PLUGIN_INFO* info, void (*plugin_entry)(maple_plugin_start_info*));

BOOL maple_register_property(MAPLE_PLUGIN_INFO* pinfo, PDT_ENT prop, BYTE_ARR* (*deserialize_callback)(PROPERTY*));

BOOL maple_write_property(MAPLE_PLUGIN_INFO* pinfo, PROPERTY* prop);

BOOL maple_read_property(MAPLE_PLUGIN_INFO* pinfo, PROP_ID id, U64 sub_id, PROPERTY* out_prop, U64 ptr_size);

//the idea here is to add way more hooks and stuff so that plugins could potentially perform any number of actions -
//pre/during/post build, possibly per file operations, who knows, i also want to have a permission/intent system which -
//forces the user to allow the plugin access to those permissions before it will be allowed to load fully

#endif //MAPLE_PLUGINS_H
