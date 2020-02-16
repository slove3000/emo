local M = class("LoadingView", View)

local Paths = {
    text = "Text",
    slider = "Slider",
}

function M:ctor(...)
    self.super.ctor(self, ...)
end

function M:OnLoaded()
    self:SetLoadingText("LOADING...")
    self:SetLoadingProgress(0)
end

function M:SetLoadingProgress(value)
    self:SetSlider(value, Paths.slider) 
end

function M:SetLoadingText(text)
    self:SetText(text, Paths.text) 
end

function M:OnClose()
end

return M