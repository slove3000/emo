local M = class("GameState")

function M:Enter()
    self:OnEnter()
end

function M:Exit()
    self:OnExit()
end

function M:Update()
    self:OnUpdate()
end

function M:OnEnter()

end

function M:OnExit()

end

function M:OnUpdate()

end

return M