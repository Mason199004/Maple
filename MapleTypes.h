#include <stdint.h>
#ifndef MAPLE_MAPLETYPES_H
#define MAPLE_MAPLETYPES_H

//types

typedef bool BOOL;

typedef int8_t I8;

typedef uint8_t U8;

typedef int16_t I16;

typedef uint16_t U16;

typedef int32_t I32;

typedef uint32_t U32;

typedef int64_t I64;

typedef uint64_t U64;

typedef struct
{
    I32 size;
    U8* str;
} LSTR;

typedef struct
{
    I32 count;
    I64 size;
    LSTR arr[];
} LSTR_ARR;

typedef struct
{
    I32 count;
    U8 arr[];
} BYTE_ARR;

typedef enum MTYPE
{
    M_BOOL,
    M_I8,
    M_U8,
    M_I16,
    M_U16,
    M_I32,
    M_U32,
    M_I64,
    M_U64,
    M_LSTR,
    M_LSTRARR,
    M_BYTEARR,
} MTYPE;

typedef struct
{
    LSTR name;
    MTYPE type;
} PROPERTY_DEF;

typedef struct
{
    LSTR name;
    void* value;
} PROPERTY;

typedef enum
{
    M_NONE,
    M_READ_ONLY,
    M_READ_WRITE,
    M_READ_APPEND
} PROPERTY_ACCESS;

typedef struct
{
    PROPERTY_DEF definition;
    PROPERTY_ACCESS access_level;
} PROPERTY_INFO;

/// the current property model might be limiting since its just K:V pairs, current goal of plugins defining
/// serialization and compiler/linker bs might be hard to implement, possible implementation is by doing
/// cursed string weirdness like "MI_COMPILERDEF_<NAME>_<FUNC>", not a big fan of this, but it could work

BOOL maple_validate_printable_string(LSTR str);

BOOL maple_lstrcmp(LSTR a, LSTR b);

#endif //MAPLE_MAPLETYPES_H
