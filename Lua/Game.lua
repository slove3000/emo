local Game = {}

function Game.Init()   
    Game.states = {}
    Game.ChangeState(GameStateID.Init)
end 

function Game.ChangeState(newStateID)
    local stateName = GameStateNames[newStateID]
    local state = Game.states[newStateID]
    if not state then
        state = require("Logic/States/" .. stateName).new()
        Game.states[newStateID] = state 
    end
    local lastState = Game.state 
    if lastState then
        lastState:Exit()
    end
    Game.state = state 
    if state then
        state:Enter()
    end 
    print("[Game]ChangeState", stateName)
end

function Game.Update()
    for i, v in ipairs(Game.states) do
        v:Update()
    end
    ActionManager.Update() 
end

function Game.OnFocus(focus)

end

function Game.OnPause(pause)

end 

function Game.Quit()
    ViewManager:CloseAll()
end

return Game