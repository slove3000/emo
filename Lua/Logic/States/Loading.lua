local M = class("Loading", GameState)

local LoadingView = require "Logic/Views/LoadingView"
local MessageBox = require "Logic/Views/MessageBox"

function M:OnEnter() 
    local messageBox
    local closeTime = 5
    local context = {
        title = "MessageBox Test",
        text = string.format("This view will close after %.0f seconds", closeTime),
        ensureFunc = function() 
            messageBox:Close()
        end,
        cancelFunc = function() 
            messageBox:Close() 
        end,
        closeFunc = function()
            local loadingView = LoadingView.new()
            loadingView:Load(Res.prefab_loading_view)
            loadingView.onloaded = function(view) 
                local frames = 100
                local frame = 0
                local action = Action.new(function(action)
                    frame = frame + 1
                    local amount = frame / frames
                    view:SetLoadingProgress(amount)
                    view:SetLoadingText(string.format("LOADING....(%.0f%%)", amount*100))
                    return frame >= frames
                end, function() 
                    view:Close()
                end)
                view:RunAction(action)
            end
        end
    }
    messageBox = MessageBox.new(context) 
    messageBox.onloaded = function() 
        local lastRemainTime = closeTime
        local completed = function(view) 
            messageBox:Close()
        end 
        local updated = function (action) 
            local remainTime = closeTime - action.elasped
            if lastRemainTime - remainTime >= 1 then
                lastRemainTime = remainTime 
                local s = string.format("This view will close after %.0f seconds", remainTime)
                messageBox:SetContentText(s)
            end
        end 
        messageBox:RunAction(WaitForSeconds(closeTime, completed, updated))
    end
    messageBox:Load(Res.prefab_message_box) 
end

function M:OnExit()
end

return M