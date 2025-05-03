local drawableSprite = require("structs.drawable_sprite")
local utils = require("utils")

local Flag = {}

Flag.name = "Celestrail/Flag"
Flag.depth = 1
Flag.canResize = {false, true}
Flag.minimumSize = {8, 24}
Flag.placements = {
    {
        name = "Celestrail Flag",
        placementType = "rectangle",
        data = {
            height = 24,
            right = false
        }
    }
}

local flagTopTexture = "flag/flagTop00"
local flagMiddleTexture = "flag/flagMid00"
local flagBottomTexture = "flag/flagBottom00"

function Flag.sprite(room, entity)
    local sprites = {}

    local height = entity.height or 24
    local tileHeight = math.floor(height / 8)

    local topSprite = drawableSprite.fromTexture(flagTopTexture, entity)
    topSprite:setJustification(0.0, 0.0)
    table.insert(sprites, topSprite)

    for i = 1, tileHeight - 2 do
        local middleSprite = drawableSprite.fromTexture(flagMiddleTexture, entity)
        middleSprite:addPosition(0, i * 8)
        middleSprite:setJustification(0.0, 0.0)
        table.insert(sprites, middleSprite)
    end

    if tileHeight > 1 then
        local bottomSprite = drawableSprite.fromTexture(flagBottomTexture, entity)
        bottomSprite:addPosition(0, (tileHeight - 1) * 8)
        bottomSprite:setJustification(0.0, 0.0)
        table.insert(sprites, bottomSprite)
    end

    return sprites
end

function Flag.rectangle(room, entity)
    return utils.rectangle(entity.x, entity.y, 8, entity.height or 24)
end

return Flag