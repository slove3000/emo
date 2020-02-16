local M = class("MessageBox", View)

local Paths = {
    text = "Content/Text",
    title = "Content/Title", 
    ensure = "Content/Options/Ensure",
    cancel = "Content/Options/Cancel",

    ensure_text = "Content/Options/Ensure/Text",
    cancel_text = "Content/Options/Cancel/Text",
}

function M:ctor(context)
    self.super.ctor(self)
    self.context = context
    self.paths = Paths
end 

function M:SetContentText(text)
    self:SetText(text, Paths.text)    
end

function M:OnLoaded()
    self:SetText(self.context.title or "Title", Paths.title)
    self:SetContentText(self.context.text or "Content")
    self:SetText(self.context.ensure_text or "Ensure", Paths.ensure_text)
    self:SetText(self.context.cancel_text or "Cancel", Paths.cancel_text) 
    local closeFunc = function()
        if self.context.cancelFunc then
            self.context.cancelFunc()
        end
    end  
    self:SetClick(closeFunc)
    self:SetClick(closeFunc, Paths.cancel) 
    self:SetClick(function()
        if self.context.ensureFunc then
            self.context.ensureFunc()
        end
    end, Paths.ensure)
end

function M:OnClose()
    if self.context.closeFunc then
        self.context.closeFunc()
    end
end

return M