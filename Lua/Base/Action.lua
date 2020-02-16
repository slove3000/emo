local M = class("Action")

function M:ctor(updater, completed)
    self.completed = completed 
    self.updater = updater
end

function M:Start() 
    ActionManager.Add(self)
    self.startTime = CS.UnityEngine.Time.time
    self.elasped = 0
end 

function M:Update()
    if self.isDone then return end
    self.elasped = CS.UnityEngine.Time.time - self.startTime 
    if self.updater and self.updater(self) then
        self:Complete()
    end  
end 

function M:Complete()
    if self.completed then
        self.completed()
    end
    self.isDone = true
    self:Dispose()
end

function M:Dispose()
    self.updater = nil
    self.completed = nil
    ActionManager.Remove(self)
end

return M