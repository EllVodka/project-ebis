package main

import (
	"fmt"
	"log"
	"os"

	"training.go/generateurScript/script"
	"training.go/generateurScript/store"
)

func main() {
	store := store.DbStore{}
	if err := store.Open(); err != nil {
		log.Fatal(err)
	}

	defer store.Close()

	scripter := script.New(&store)

	opChargementScript := scripter.GetOpChargement()

	if err := store.Insert(opChargementScript); err != nil {
		log.Fatal(err)
	}

	incidentScript := scripter.GetIncident()

	entretienScript := scripter.GetEntretien()

	detailEntretienScript := scripter.GetDetailEntretien()

	resultat := fmt.Sprintf("%v\n\n%v\n\n%v\n\n%v", opChargementScript, incidentScript, entretienScript, detailEntretienScript)

	fmt.Println("Insert done")

	file, err := os.OpenFile("script.txt", os.O_CREATE|os.O_TRUNC, 0600)
	if err != nil {
		log.Fatal(err)
	}

	defer file.Close()

	_, err = file.WriteString(resultat)
	if err != nil {
		log.Fatal(err)
	}
}
