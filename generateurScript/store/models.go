package store

import (
	"fmt"
	"time"
)

type Techniciens []Technicien

type Borne struct {
	IdBorne        int `db:"id"`
	CodeTypeCharge int `db:"codetypecharge"`
}

type OpChargement struct {
	IdControle int       `db:"idcontrole"`
	IdBorne    int       `db:"idborne"`
	DateDebut  time.Time `db:"dateheuredebut"`
}

type Technicien struct {
	IdTechnicien      int       `db:"idtechnicien"`
	DateMiseEnService time.Time `db:"datemiseenservice"`
	IdBorne           int       `db:"idborne"`
}

type ElementEntretien struct {
	IdEntretien int `db:"identretien"`
	IdElement   int `db:"idelement"`
	Annee       int `db:"annee"`
	Mois        int `db:"mois"`
	Jour        int `db:"jour"`
	Heure       int `db:"heure"`
}

func (t Techniciens) AddOneYear() {
	for i := range t {
		t[i].DateMiseEnService = t[i].DateMiseEnService.AddDate(1, 0, 0)
	}
}

func (b *Borne) GetPuissanceTypeCharge() int {
	switch b.CodeTypeCharge {
	case 1:
		return 3
	case 2:
		return 24
	case 3:
		return 50
	default:
		return 3
	}
}

func (oc *OpChargement) ToString() string {
	return fmt.Sprintf("Controle echoue sur le controle n°%v", oc.IdControle)
}

func (oc *OpChargement) GetIdIncident() int {
	switch oc.IdControle {
	case 1:
		return 3
	case 2:
		return 1
	case 3:
		return 4
	default:
		return 1
	}
}
