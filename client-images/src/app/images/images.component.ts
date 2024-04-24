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

  constructor(public http:HttpClient) { }

  ngOnInit(): void {
    this.getPictures();
  }

  pictures : Picture[] = [];

  // Ce ViewChild est utilisé pour accéder au fichier qui a été sélectionné par l'utilisateur
  @ViewChild("fileUploadViewChild", {static:false}) pictureInput ?: ElementRef;

  async uploadPicture(): Promise<void> {
    if(this.pictureInput == undefined){
      console.log("Input HTML non chargé.");
    }

    // TO DO: [Étape 2] Faire une requête post à votre serveur pour ajouter l'image qui a été sélectionnée
    // TO DO: [Étape 2] Votre serveur doit retourner l'instance de Picture nouvellement créée que vous devrez ajouter à votre array de Picture
  }

  async getPictures(): Promise<void> {
    // TO DO: [Étape 4] Faire une requête à votre serveur pour obtenir les images
  }

  async deletePicture(picture:Picture): Promise<void> {
    // TO DO: [Étape 4] Faire une requête à votre serveur pour supprimer une image

    // Une fois que l'image est effacée, il faut mettre à jour les images que l'on affiche
    await this.getPictures();
  }

}
