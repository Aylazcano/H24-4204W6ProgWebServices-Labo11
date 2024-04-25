import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Picture } from '../models/picture';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-images',
  templateUrl: './images.component.html',
  styleUrls: ['./images.component.css']
})
export class ImagesComponent implements OnInit {

  constructor(public http: HttpClient) { }

  ngOnInit(): void {
    this.getPictures();
  }

  pictures: Picture[] = [];

  // Ce ViewChild est utilisé pour accéder au fichier qui a été sélectionné par l'utilisateur
  @ViewChild("fileUploadViewChild", { static: false }) pictureInput?: ElementRef;

  async uploadPicture(): Promise<void> {
    if (this.pictureInput == undefined) {
      console.log("Input HTML non chargé.");
      return;
    }

    // TO DO: [Étape 2] Faire une requête post à votre serveur pour ajouter l'image qui a été sélectionnée
    // TO DO: [Étape 2] Votre serveur doit retourner l'instance de Picture nouvellement créée que vous devrez ajouter à votre array de Picture
    let file = this.pictureInput.nativeElement.files[0];
    if (file == null) {
      console.log("Input HTML ne contient aucune image.")
      return;
    }

    // Créez un objet FormData pour envoyer le fichier au serveur
    let formData = new FormData();
    formData.append("birdImg", file, file.name);

    try {
      let x = await lastValueFrom(this.http.post<any>("http://localhost:7243/api/Pictures/PostPicture", formData));
      console.log(x);

      // Ajouter le Picture nouvellement créé à votre array de Picture
      this.pictures.push(x);

      this.getPictures(); // Met à jour la liste des images après l'ajout d'une nouvelle image
    } catch (error) {
      console.error("Une erreur s'est produite lors de l'envoi de l'image :", error);
    }

  }

  async getPictures(): Promise<void> {
    // TO DO: [Étape 4] Faire une requête à votre serveur pour obtenir les images
    let x = await lastValueFrom(this.http.get<Picture[]>("http://localhost:7243/api/Pictures/GetPictures"));
    console.log(x);
    this.pictures = x;
  }

  async deletePicture(picture: Picture): Promise<void> {
    // TO DO: [Étape 4] Faire une requête à votre serveur pour supprimer une image
    let x = await lastValueFrom(this.http.delete<any>("http://localhost:7243/api/Pictures/DeletePicture/" + picture.id))
    console.log(x);
    // Une fois que l'image est effacée, il faut mettre à jour les images que l'on affiche
    await this.getPictures();
  }

}
