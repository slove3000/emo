local ActionManager = {
    actions = {}
}

function ActionManager.Add(action)
    table.insert(ActionManager.actions, action)
end

function ActionManager.Remove(action)
    for i, v in ipairs(ActionManager.actions) do
        if v == action then 
            table.remove(ActionManager.actions, i)
            break;
        end
    end
end

function ActionManager.Update()
    for i, v in ipairs(ActionManager.actions) do
        v:Update()
    end
end


return ActionManager