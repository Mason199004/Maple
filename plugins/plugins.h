#include "../MapleTypes.h"
#include "../Project/MapleProj.h"
#ifndef MAPLE_PLUGINS_H
#define MAPLE_PLUGINS_H

typedef struct
{
    U64 plugin_version; //x.x.x.x each value is 2 bytes
    LSTR name;
    LSTR description;
} maple_plugin_info;

typedef enum
{
    MP_UNLOADED,
    MP_LOADED,
    MP_READY
} maple_plugin_load_state;

typedef struct
{
    U64 struct_size;
    BOOL (*maple_register_property)(PROPERTY_DEF prop, PROPERTY_ACCESS external_access_level);
    BOOL (*maple_write_property)(PROPERTY* prop);
    BOOL (*maple_read_property)(LSTR property_name, PROPERTY* out_prop);
    BOOL (*maple_get_property_info)(LSTR property_name, PROPERTY_INFO* out_info);
    BOOL (*maple_list_properties)(LSTR_ARR* out_list);
    void (*maple_plugin_log_error)(LSTR str);
    void (*maple_plugin_log_warning)(LSTR str);
    void (*maple_plugin_log_info)(LSTR str);
    //will contain function pointers to all maple plugin system functions, struct in passed into a plugins init function
} maple_plugin_procs;

typedef struct
{
    void (*init_proc)(maple_plugin_procs proc_struct);
    maple_plugin_load_state state;
} maple_plugin_data;

typedef struct
{
    maple_plugin_info info;
    maple_plugin_data data;
} maple_plugin_KV;

typedef struct
{
    U64 num_plugins;
    U64 plugin_capacity;
    maple_plugin_KV* plugins;
} maple_plugins;

BOOL maple_register_property(PROPERTY_DEF prop, PROPERTY_ACCESS external_access_level);

BOOL maple_write_property(PROPERTY* prop);

BOOL maple_read_property(LSTR property_name, PROPERTY* out_prop);

BOOL maple_get_property_info(LSTR property_name, PROPERTY_INFO* out_info);

BOOL maple_list_properties(LSTR_ARR* out_list);

void maple_plugin_log_error(LSTR str);

void maple_plugin_log_warning(LSTR str);

void maple_plugin_log_info(LSTR str);

BOOL maple_create_plugin(maple_plugin_info info, void (*init_proc)(maple_plugin_procs));

BOOL validate_plugin(maple_plugin_info* info);

#endif //MAPLE_PLUGINS_H
