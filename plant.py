def enum(**enums):
    return type('Enum', (), enums)
month_enum = enum(
    JAN=0,
    FEB=1,
    MAR=2,
    APR=3,
    MAY=4,
    JUN=5,
    JUL=6,
    AUG=7,
    SEP=8,
    OCT=9,
    NOV=10,
    DEC=11
    )
plant_enum = enum(
    ALIVE=0,
    SEED=1,
    DEAD=2
    )

color_rep_enum = enum(
    WINTER=0,
    FLOWER=1,
    GREEN=2
    )

sun_enum = enum(
    SHADE=0,
    PARTIAL=1,
    FULL=2
    )

class Plant(object):
    

    def __init__(self, x, y, state, width=0, height=0):
        self.x = x
        self.y = y
        self.state = state
        self.width_radius = width_radius
        self.height = height
        # super(Plant, self).__init__(self, x, y, state, width, height)

    def color_rep(self):
        raise NotImplementedError

    def height(self):
        raise NotImplementedError

    def width_radius(self):
        raise NotImplementedError

class BearGrass(Plant):
    def __init__(self, x, y, state, width=0, height=0):
        super(Plant, self).__init__(self, x, y, state, width, height)
class LittleBluestem(Plant):
    def __init__(self, x, y, state, width=0, height=0):
        super(Plant, self).__init__(self, x, y, state, width, height)
class Yucca(Plant):
    def __init__(self, x, y, state, width=0, height=0):
        super(Plant, self).__init__(self, x, y, state, width, height)
