GameStateID = {
    Init = 1,
    Loading = 2, 
}

GameStateNames = {
    [1] = "Initialize",
    [2] = "Loading", 
}

-- for cs
Assets = CS.xasset.Assets
GameObject = CS.UnityEngine.GameObject
-- for lua
ActionManager = require "Mgrs/ActionManager"
ViewManager = require "Mgrs/ViewManager"
Action = require "Base/Action"
View = require "Base/View"
GameState = require "Base/GameState"
Game = require "Game"

function WaitForSeconds(seconds, completed, updated)
    return Action.new(function(action)
        if updated then
            updated(action)
        end
        return action.elasped >= seconds
    end, completed)
end

function IntValue(number)
    return number - (number % 1)
end 