#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include "MapleProj.h"

PDT_ENT* PropList;
BYTE_ARR* (**CallbackList)(PROPERTY*);
I32 PropCount;
I32 AllocCount;

void maple_init_proj_system()
{
    PropCount = 0;
    AllocCount = 8;
    PropList = malloc(sizeof(PDT_ENT) * (AllocCount));
    CallbackList = malloc(sizeof(BYTE_ARR*) * (AllocCount));
}

BOOL maple_check_for_existing(const PDT_ENT* prop)
{
    for (int i = 0; i < PropCount; ++i)
    {
        if (memcmp(PropList[i].id, prop->id, sizeof(prop->id)) == 0)
        {
            return true;
        }
    }
    return false;
}

BOOL maple_inform_new_property(PDT_ENT prop, BYTE_ARR* (*deserialize_callback)(PROPERTY*))
{
    if (PropCount + 1 > AllocCount)
    {
        PDT_ENT* temp = realloc(PropList, sizeof(PDT_ENT) * (AllocCount * 2));
        BYTE_ARR* (**temp2)(PROPERTY*) = realloc(CallbackList, sizeof(BYTE_ARR*) * (AllocCount * 2));

        if (temp == NULL || temp2 == NULL)
        {
            //todo: handle error
        }

        PropList = temp;
        CallbackList = temp2;
        AllocCount *= 2;
    }

    PropList[PropCount] = prop;
    CallbackList[PropCount++] = deserialize_callback;
    return true;
}
