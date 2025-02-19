#ifndef MAPLE_PLUGINS_H
#define MAPLE_PLUGINS_H
#include "../MapleTypes.h"
#include "../Project/MapleProj.h"

typedef struct
{
    U64 plugin_version; //x.x.x.x each value is 2 bytes
    LSTR name;
    LSTR description;
} maple_plugin_info;

#define MAKE_VERNUM(sig, major, minor, patch) ((((U64)(U16)sig) << 48) | (((U64)(U16)major) << 32) | (((U64)(U16)minor) << 16) | ((U16)patch))

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
    BOOL (*init_proc)(maple_plugin_procs proc_struct);
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


//gcc __builtin_return_address + dladdr for getting the handle to calling modules for log purposes and permission checking
//core plugins will be annoying since this wont work for them

BOOL maple_register_property(PROPERTY_DEF prop, PROPERTY_ACCESS external_access_level);

BOOL maple_write_property(PROPERTY prop);

BOOL maple_read_property(LSTR property_name, PROPERTY* out_prop);

BOOL maple_get_property_info(LSTR property_name, PROPERTY_INFO* out_info);

BOOL maple_list_properties(LSTR_ARR* out_list);

void maple_plugin_log_error(LSTR str);

void maple_plugin_log_warning(LSTR str);

void maple_plugin_log_info(LSTR str);

BOOL maple_create_plugin(maple_plugin_info info, void (*init_proc)(maple_plugin_procs));

BOOL validate_plugin(maple_plugin_info* info);

#endif //MAPLE_PLUGINS_H
