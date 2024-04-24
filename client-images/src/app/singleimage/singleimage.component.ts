import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-singleimage',
  templateUrl: './singleimage.component.html',
  styleUrls: ['./singleimage.component.css']
})
export class SingleImageComponent implements OnInit {

  pictureId : string | null = null;

  constructor(public route : ActivatedRoute, public http:HttpClient) { }

  ngOnInit() {
    // TO DO: [Étape 4] Il faut récupérer l'id de l'image qui va être utilisé pour obtenir l'image haute résolution du serveur
  }

}
