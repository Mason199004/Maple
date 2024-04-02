#include "../MapleTypes.h"
#ifndef MAPLE_MAPLEPROJ_H
#define MAPLE_MAPLEPROJ_H

/*
 * Maple Build System binary configuration format; build.maple
 *  HEADER:
 *    5 byte magic ascii"MAPLE"
 *    Property Descriptor Table: describes all properties used by maple, including those added by plugins
 *  PROPERTY DATA:
 *    U64 length of section
 *    [list of properties]
 *  DATA STORE: \\heap like structure containing any data that could not be contained in a property
 *    U64 length of section
 *    [data]
 */


typedef enum MTYPE
{
    MBOOL,
    MI8,
    MU8,
    MI16,
    MU16,
    MI32,
    MU32,
    MI64,
    MU64,
    MLSTR,
    MLSTRARR,
    MBYTEARR,
    MCUSTOM
} MTYPE;

//property descriptor table

typedef U8 PROP_ID[16];

typedef struct
{
    PROP_ID id;
    MTYPE type;
    BOOL can_have_many;
} PDT_ENT;

typedef struct
{
    I32 ent_count;
    PDT_ENT entries[];
} PDT;

typedef struct
{
    PROP_ID id;
    U32 sub_id;
    U32 size;
    U8 data[];
} PROPERTY;

//data store ; do later todo

#endif //MAPLE_MAPLEPROJ_H
