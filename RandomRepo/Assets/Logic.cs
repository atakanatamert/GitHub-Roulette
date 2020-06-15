using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    public Text username;
    public Text repo;
    public GameObject Images;
    UserList userList;
    RepoList repoList;
    float positionX = -6.39f;
    float positionY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Images.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetRandom()
    {
        Images.GetComponent<Rigidbody2D>().isKinematic = true;
        Images.GetComponent<Rigidbody2D>().isKinematic = false;
        if (Images.transform.childCount > 0)
        {
            foreach (Transform child in Images.transform)
            {
                Destroy(child.gameObject);
            }
        }

        Images.transform.position = Vector3.zero;
        positionX = -6.39f;
        username.text = "Username: ";
        repo.text = "Repository: ";
        Images.SetActive(false);
        GameObject.Find("Button").GetComponent<Button>().enabled = false;
        StartCoroutine(GetUsers());
    }

    IEnumerator GetUsers()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.github.com/users?since=2020");
        yield return www.SendWebRequest();

        if (www.responseCode == 200)
        {
            userList = JsonUtility.FromJson<UserList>("{\"users\":" + www.downloadHandler.text + "}");
        }
        yield return StartCoroutine(Roll());
    }
    IEnumerator Roll()
    {
        int randomIndexUser = UnityEngine.Random.Range(0, userList.users.Length);
        yield return StartCoroutine(GetRandomRepo(userList.users[randomIndexUser].login));
        yield return StartCoroutine(PickRepo(randomIndexUser));
        Images.SetActive(true);
        yield return new WaitForSeconds(2);
        Images.GetComponent<Rigidbody2D>().AddForce(new Vector2(-45f, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(11);
        username.text = "Username: " + userList.users[randomIndexUser].login;

        try
        {
            int randomIndex = UnityEngine.Random.Range(0, repoList.repos.Length);
            //Debug.Log("Repository: " + repoList.repos[randomIndex].url);
            repo.text = "Repository: " + repoList.repos[randomIndex].html_url;
            TextEditor te = new TextEditor();
            te.text = repoList.repos[randomIndex].html_url;
            te.SelectAll();
            te.Copy();
        }
        catch (Exception e)
        {
            repo.text = "Uhm, no repo ??????";
        }
        GameObject.Find("Button").GetComponent<Button>().enabled = true;

    }

    IEnumerator PickRepo(int randomIndex)
    {
        //  int randomIndex = UnityEngine.Random.Range(0, userList.users.Length);
        //  username.text = "Username: " + userList.users[randomIndex].login;

        for (int i = 0; i < 32; i++)
        {
            if (i == 27)
            {
                yield return StartCoroutine(GetProfilePic(userList.users[randomIndex].avatar_url));
            }
            else
            {
                int randomUser = UnityEngine.Random.Range(0, userList.users.Length);
                yield return StartCoroutine(GetProfilePic(userList.users[randomUser].avatar_url));
            }
        }


        yield return 0;
    }

    IEnumerator GetRandomRepo(string user)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.github.com/users/" + user + "/repos?per_page=10");
        yield return www.SendWebRequest();
        if (www.responseCode == 200)
        {
            repoList = JsonUtility.FromJson<RepoList>("{\"repos\":" + www.downloadHandler.text + "}"); ;
        }
    }

    IEnumerator GetProfilePic(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.responseCode == 200)
        {
            //Debug.Log(localUrl);
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3(positionX, positionY, 0);
            plane.transform.rotation = Quaternion.Euler(-270, 0, 180);
            plane.transform.localScale = new Vector3(0.334122f, 0.334122f, 0.334122f);
            DestroyImmediate(plane.GetComponent<MeshCollider>());
            plane.AddComponent<CircleCollider2D>();

            Renderer rend = plane.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Unlit/Texture"));
            rend.material.mainTexture = DownloadHandlerTexture.GetContent(www);
            plane.transform.parent = Images.transform;
            positionX += 3.67f;
        }

    }
}




[System.Serializable]
public class RepoList
{
    public Repo[] repos;
}


[System.Serializable]
public class Repo
{
    public User user;
    public string html_url;
    public string description;
    public bool fork;
    public string url;
    public string forks_url;
    public string keys_url;
    public string collaborators_url;
    public string teams_url;
    public string hooks_url;
    public string issue_events_url;
    public string events_url;
    public string assignees_url;
    public string branches_url;
    public string tags_url;
    public string blobs_url;
    public string git_tags_url;
    public string git_refs_url;
    public string trees_url;
    public string statuses_url;
    public string languages_url;
    public string stargazers_url;
    public string contributors_url;
    public string subscribers_url;
    public string subscription_url;
    public string commits_url;
    public string git_commits_url;
    public string comments_url;
    public string issue_comment_url;
    public string contents_url;
    public string compare_url;
    public string merges_url;
    public string archive_url;
    public string downloads_url;
    public string issues_url;
    public string pulls_url;
    public string milestones_url;
    public string notifications_url;
    public string labels_url;
    public string releases_url;
    public string deployments_url;
    public string created_at;
    public string updated_at;
    public string pushed_at;
    public string git_url;
    public string ssh_url;
    public string clone_url;
    public string svn_url;
    public string homepage;
    public int size;
    public int stargazers_count;
    public int watchers_count;
    public string language;
    public bool has_issues;
    public bool has_projects;
    public bool has_downloads;
    public bool has_wiki;
    public bool has_pages;
    public int forks_count;
    public string mirror_url;
    public bool archived;
    public bool disabled;
    public int open_issues_count;
    public string license;
    public int forks;
    public int open_issues;
    public int watchers;
    public string default_branch;
}

[System.Serializable]
public class UserList
{
    public User[] users;
}


[System.Serializable]
public class User
{
    public string login;
    public int id;
    public string node_id;
    public string avatar_url;
    public string gravatar_id;
    public string url;
    public string html_url;
    public string followers_url;
    public string following_url;
    public string gists_url;
    public string starred_url;
    public string subscriptions_url;
    public string organizations_url;
    public string repos_url;
    public string events_url;
    public string received_events_url;
    public string type;
    public bool site_admin;

}
