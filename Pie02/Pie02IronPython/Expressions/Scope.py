
class Scope(object):
    """description of class"""

    AllTypes = []

    def __init__(self):
        self.LocalVariables = []
        
    def FindVariable(self, name):

        for lv in self.LocalVariables:
            if lv == name:
                return lv
        return None

    def Copy(self):
        s = Scope()
        s.LocalVariables = list(self.LocalVariables)
        return s