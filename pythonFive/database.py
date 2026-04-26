from sqlalchemy import create_engine, Column, Integer, String, DateTime, ForeignKey
from sqlalchemy.orm import declarative_base, sessionmaker, relationship

MYSQL_URL = "mysql+mysqlconnector://root:1234@localhost/scientific_foundation"
POSTGRE_URL = "postgresql://postgres:1234@localhost:5432/science_db"

Base = declarative_base()

class Scientist(Base):
    __tablename__ = "scientists"
    id_scient = Column(Integer, primary_key=True)
    fio_scient = Column(String(255))
    grants = relationship("Grant", back_populates="scientist")

class Direction(Base):
    __tablename__ = "directions"
    id_direction = Column(Integer, primary_key=True)
    name_direction = Column(String(255))
    grants = relationship("Grant", back_populates="direction")

class Grant(Base):
    __tablename__ = "grants"
    id_grants = Column(Integer, primary_key=True)
    name_theme = Column(String(255))
    summa = Column(Integer)
    date_start = Column(DateTime)
    date_end = Column(DateTime)
    organization = Column(String(255))
    id_scient = Column(Integer, ForeignKey("scientists.id_scient"))
    id_direction = Column(Integer, ForeignKey("directions.id_direction"))
    
    scientist = relationship("Scientist", back_populates="grants")
    direction = relationship("Direction", back_populates="grants")

def get_session(db_type="mysql"):
    url = MYSQL_URL if db_type == "mysql" else POSTGRE_URL
    engine = create_engine(url)
    Base.metadata.create_all(bind=engine)
    return sessionmaker(bind=engine)()