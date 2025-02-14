#include "../MapleTypes.h"
#include "../Project/MapleProj.h"
#ifndef MAPLE_PLUGINS_H
#define MAPLE_PLUGINS_H

typedef struct
{
    U64 plugin_version; //x.x.x.x each value is 2 bytes
    LSTR name;
    LSTR description;
} MAPLE_PLUGIN_INFO;
// probably not going to use the struct anymore, will likely keep the same info but as multiple symbols in the
// plugin dll's, lookup using dlsym or wtv


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
} maple_plugin_start_info;

BOOL maple_register_property(PROPERTY_DEF prop, PROPERTY_ACCESS external_access_level);

BOOL maple_write_property(PROPERTY* prop);

BOOL maple_read_property(LSTR property_name, PROPERTY* out_prop);

BOOL maple_get_property_info(LSTR property_name, PROPERTY_INFO* out_info);

BOOL maple_list_properties(LSTR_ARR* out_list);

void maple_plugin_log_error(LSTR str);

void maple_plugin_log_warning(LSTR str);

void maple_plugin_log_info(LSTR str);



#endif //MAPLE_PLUGINS_H
