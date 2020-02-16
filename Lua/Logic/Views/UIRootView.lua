local M = class("UIRootView", View)

function M:ctor(...)
    self.super.ctor(self, ...)
end

function M:OnLoaded()
    GameObject.DontDestroyOnLoad(self.gameObject)
    self.canvasRoot = self.gameObject.transform:Find("Canvas")
end

function M:OnClose()
    
end

return M