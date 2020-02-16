local M = class("Initialize", GameState)
local UIRootView = require "Logic/Views/UIRootView"

function M:OnEnter()
    local view = UIRootView.new()
    view.onloaded = function()
        Game.ChangeState(GameStateID.Loading)
    end
    view:Load(Res.prefab_uiroot)
end

function M:OnExit()
end

return M