local M = class("View")

function M:Find(path)
    local trans
    if path == nil then
        trans = self.transform
    else
        trans = self.transform:Find(path)
    end 
    return trans
end

function M:ctor()
    self.sliders = {}
    self.texts = {} 
    self.actions = {}   
    ViewManager.Add(self)
end

function M:Load(assetPath)
    self.asset = Assets.LoadAsync(assetPath, typeof(GameObject))
    self.asset.completed = function(a) 
        if not self.closed then
            local prefab = a.asset
            local go  = GameObject.Instantiate(prefab)  
            go.name = prefab.name   
            self.gameObject = go 
            self.luaBehaviour = self.gameObject:AddComponent(typeof(CS.emo.LuaBehaviour))
            self.transform = self.gameObject.transform
            if self.onloaded then
                self.onloaded(self)
            end
            self:OnLoaded() 
            if ViewManager.rootView == nil then
                ViewManager.rootView = self
            else
                self:SetParent(ViewManager.rootView.canvasRoot)
            end
            self.loaded = true 
        end
    end 
end 

function M:Close()
    ViewManager.Remove(self)
    self.onloaded = nil
    for i, v in ipairs(self.actions) do
        v:Dispose()
    end 
    self:OnClose()
    GameObject.Destroy(self.gameObject)
    self.asset:Release()
    for key, value in pairs(self) do
        value = nil
    end
    self.closed = true
end 

function M:RemoveAction(action) 
    for i, v in ipairs(self.actions) do
        if v == action then
            table.remove(self.actions, i)
            break;
        end
    end 
end

function M:RunAction(action) 
    table.insert(self.actions, action)
    local completed = action.completed
    action.completed = function() 
        self:RemoveAction(action)
        completed()
    end
    action:Start()
end 

function M:SetParent(parent)
    self.gameObject.transform:SetParent(parent, false)
end

function M:OnLoaded()
    
end

function M:SetText(value, path)  
    self.luaBehaviour:SetText(value, path)
    self.texts[path] = value
end

function M:GetText(path)
    return self.texts[path]
end

function M:SetSlider(value, path)
    self.luaBehaviour:SetSlider(value, path) 
    self.sliders[path] = value
end

function M:SetClick(value, path)
    self.luaBehaviour:SetButtonClick(path, value)
end

function M:GetSlider(path)
    return self.sliders[path]
end 

function M:OnClose()

end 

return M