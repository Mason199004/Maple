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
    U8 str[];
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

BOOL maple_validate_printable_string(LSTR* str);

BOOL maple_lstrcmp(LSTR* a, LSTR* b);

#endif //MAPLE_MAPLETYPES_H
