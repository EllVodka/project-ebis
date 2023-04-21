package script

import (
	"fmt"
	"log"
	"math/rand"
	"strings"
	"time"

	"training.go/generateurScript/store"
)

const datetime string = "2006-01-02 15:04:05"

type Script struct {
	store store.Store
}

type Scripter interface {
	GetOpChargement() string
	GetIncident() string
	GetEntretien() string
	GetDetailEntretien() string
}

func New(storer store.Store) *Script {
	return &Script{
		store: storer,
	}
}

func (s *Script) GetOpChargement() string {
	bornes, err := s.store.GetBorne()
	if err != nil {
		log.Fatal(err)
	}

	baseReq := "INSERT INTO operationrechargement (dateheuredebut,dateheurefin,idcontrole,idborne,nbkwheures) VALUES\n"
	rand.Seed(time.Now().UnixNano())

	for _, b := range bornes {
		nbLoop := rand.Intn(8-4) + 4
		for i := 1; i <= nbLoop; i++ {
			anneeDebut := rand.Intn(2023-2018) + 2018
			moisDebut := rand.Intn(13-1) + 1
			jourDebut := rand.Intn(29-1) + 1
			heureDebut := rand.Intn(24-1) + 1
			dateDebut := time.Date(anneeDebut, time.Month(moisDebut), jourDebut, heureDebut, 0, 0, 0, time.UTC)

			dateFin := dateDebut
			controle := "null"
			nbKwH := 0
			if rand.Intn(2) == 1 { // Si le controle a reussi
				duree := rand.Intn(181-30) + 30
				dateFin = dateDebut.Add(time.Duration(duree) * time.Minute)
				nbKwH = b.GetPuissanceTypeCharge() * (duree / 60)
			} else {
				controle = fmt.Sprintf("%v", rand.Intn(4-1)+1)
			}

			baseReq += fmt.Sprintf(
				"('%v','%v',%v,%v,%v),\n",
				dateDebut.Format(datetime),
				dateFin.Format(datetime),
				controle,
				b.IdBorne,
				nbKwH,
			)
		}
	}

	baseReq = strings.Trim(baseReq, "\n,")
	return fmt.Sprintf("%v;", baseReq)
}

func (s *Script) GetIncident() string {
	opChargement, err := s.store.GetOpChargementControled()
	if err != nil {
		log.Fatal(err)
	}

	baseReqIncident := "INSERT INTO incident (detail, annee, mois, jour, heures, idborne, idtypeincident) VALUES\n"

	for _, oc := range opChargement {
		baseReqIncident += fmt.Sprintf(
			"(\"%v\",%v,%v,%v,%v,%v,%v),\n",
			oc.ToString(),
			oc.DateDebut.Year(),
			int(oc.DateDebut.Month()),
			oc.DateDebut.Day(),
			oc.DateDebut.Hour(),
			oc.IdBorne,
			oc.GetIdIncident(),
		)
	}

	baseReqIncident = strings.Trim(baseReqIncident, "\n,")
	return fmt.Sprintf("%v;", baseReqIncident)
}

func (s *Script) GetEntretien() string {
	baseReq := "INSERT INTO entretien (titre, annee, mois, jour, heure, idborne, idtechnicien) VALUES\n"

	techs, err := s.store.GetTechnicienAndBorne()
	if err != nil {
		log.Fatal(err)
	}
	for i := 0; i <= 5; i++ {
		for _, t := range techs {
			baseReq += fmt.Sprintf(
				"('entretien annuel',%v,%v,%v,%v,%v,%v),\n",
				t.DateMiseEnService.Year(),
				int(t.DateMiseEnService.Month()),
				t.DateMiseEnService.Day(),
				t.DateMiseEnService.Hour(),
				t.IdBorne,
				t.IdTechnicien,
			)
		}

		techs.AddOneYear()
	}

	baseReq = strings.Trim(baseReq, "\n,")
	return fmt.Sprintf("%v;", baseReq)
}

func (s *Script) GetDetailEntretien() string {
	baseReq := "INSERT INTO detailentretien (idElement,idEntretien,annee,mois,jour,heure) VALUES\n"

	ees, err := s.store.GetEntretienElement()
	if err != nil {
		log.Fatal(err)
	}

	rand.Seed(time.Now().UnixNano())

	for _, ee := range ees {
		if rand.Intn(2) == 1 {
			baseReq += fmt.Sprintf(
				"(%v,%v,%v,%v,%v,%v),\n",
				ee.IdElement,
				ee.IdEntretien,
				ee.Annee,
				ee.Mois,
				ee.Jour,
				ee.Heure,
			)
		} else {
			baseReq += fmt.Sprintf("(%v,%v,null,null,null,null),\n", ee.IdElement, ee.IdEntretien)
		}

	}

	baseReq = strings.Trim(baseReq, "\n,")
	return fmt.Sprintf("%v;", baseReq)
}
