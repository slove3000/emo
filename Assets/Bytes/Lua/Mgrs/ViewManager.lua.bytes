local ViewManager = {
    views = {}, 
} 

function ViewManager.Add(view)
    table.insert(ViewManager.views, view)
end 

function ViewManager.Remove(view)
    for i, v in ipairs(ViewManager.views) do
        if v == view then
            table.remove(ViewManager.views, i)
            return
        end
    end
end
 
return ViewManager