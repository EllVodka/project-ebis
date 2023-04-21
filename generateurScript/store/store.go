package store

import (
	"log"

	_ "github.com/go-sql-driver/mysql"
	"github.com/jmoiron/sqlx"
)

type Store interface {
	Open() error
	Close() error

	GetBorne() ([]Borne, error)
	GetOpChargementControled() ([]OpChargement, error)
	GetTechnicienAndBorne() (Techniciens, error)
	GetEntretienElement() ([]ElementEntretien, error)
	Insert(query string) error
}

type DbStore struct {
	db *sqlx.DB
}

func (store *DbStore) Open() error {
	db, err := sqlx.Connect("mysql", "root:root@(localhost:3306)/ebis_v2?parseTime=true")
	if err != nil {
		return err
	}
	log.Println("Connected to DB")
	store.db = db
	return nil
}

func (store *DbStore) Close() error {
	return store.db.Close()
}

func (store *DbStore) GetEntretienElement() ([]ElementEntretien, error) {
	var ee []ElementEntretien

	err := store.db.Select(&ee, `SELECT e.id as idelement, ent.id as identretien,
	ent.annee, ent.mois, ent.jour, ent.heure 
	FROM element e 
	JOIN borne b ON b.id = e.idborne
	JOIN entretien ent ON ent.idborne = b.id;`)
	if err != nil {
		return nil, err
	}

	return ee, nil
}

func (store *DbStore) GetBorne() ([]Borne, error) {
	var bornes = []Borne{}
	err := store.db.Select(&bornes, "SELECT id,codetypecharge FROM borne")
	if err != nil {
		return nil, err
	}

	return bornes, nil
}

func (store *DbStore) GetOpChargementControled() ([]OpChargement, error) {
	var opChargement []OpChargement

	err := store.db.Select(&opChargement, `SELECT idborne, idcontrole, dateheuredebut 
	FROM operationrechargement WHERE idcontrole IS NOT null;`)
	if err != nil {
		return nil, err
	}

	return opChargement, nil
}

func (store *DbStore) GetTechnicienAndBorne() (Techniciens, error) {
	var tech []Technicien

	err := store.db.Select(&tech, `SELECT b.id as idborne,b.datemiseenservice,t.id as idtechnicien FROM technicien t
	JOIN secteur s ON t.idsecteur = s.id
	JOIN station st ON s.id = st.idsecteur
	JOIN borne b ON b.idstation = st.id;;`)
	if err != nil {
		return nil, err
	}

	return tech, nil
}

func (store *DbStore) Insert(query string) error {
	_, err := store.db.Exec(query)
	if err != nil {
		return err
	}

	return nil
}
