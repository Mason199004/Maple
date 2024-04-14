#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include "MapleProj.h"
#include "../logging/logging.h"

BYTE_ARR* (**CallbackList)(PROPERTY*);
PDT* Properties;

PROPERTY* PropList;
U64 PLSize;

void maple_init_proj_system()
{
    Properties = malloc(sizeof(PDT));
    CallbackList = malloc(sizeof(BYTE_ARR*) * (0));
    PropList = malloc(1);
    PLSize = 0;
}

BOOL maple_check_for_existing(const PDT_ENT* prop)
{
    for (int i = 0; i < Properties->ent_count; ++i)
    {
        if (memcmp(Properties->entries[i].id, prop->id, sizeof(prop->id)) == 0)
        {
            return true;
        }
    }
    return false;
}

BOOL maple_inform_new_property(PDT_ENT prop, BYTE_ARR* (*deserialize_callback)(PROPERTY*))
{

    PDT* temp = realloc(Properties, sizeof(PDT) + (sizeof(PDT_ENT) * (Properties->ent_count + 1)));
    BYTE_ARR* (**temp2)(PROPERTY*) = realloc(CallbackList, sizeof(BYTE_ARR*) * (Properties->ent_count + 1));

    if (temp == NULL || temp2 == NULL)
    {
        maple_system_panic("ProjectSystem", "realloc failed!");
    }

    Properties = temp;
    CallbackList = temp2;

    Properties->entries[Properties->ent_count] = prop;
    CallbackList[Properties->ent_count++] = deserialize_callback;
    return true;
}

