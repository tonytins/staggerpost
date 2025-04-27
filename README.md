# Stagger Post

This is a very simple console application that suggests a list of times to post news articles within a randomized 2-3 hour and 30-minute delay to avoid conflicts. This keeps thing flowing at an organic and slow pace.

It is not recommended for use with breaking news, of course. Instead, you should focus on overlooked local news or op-eds. Of course, this is just covering general news. It is really good niche communities.

## 🛠 Features

* [x] User Input and Name Bias
* [x] JSON Export

## 📩 Exporting

Once it generates a list of times, you can retry or export it as a JSON format. If you export, you're requested to select from a number of topics or default to none if no selection is made. Upon export, you can start over or exit.

Choice selection is based on the Y/N keys. ``Enter`` works the same as ``Y`` while pressing any other key is the equivalent of ``N``. You can make mistakes, but it expects failure.

An optional ``config.toml`` file allows for further customization of file name, directory, and topics.

### Example

```json
[
  {
    "community": "games@lemmy.world",
    "date": "3/22/2025",
    "times": [
      "1:34 PM",
      "3:03 PM",
      "5:27 PM",
      "9:13 PM",
      "11:37 PM"
    ]
  }
]
```

## 📅 Support & Update Cycle

| Type         | Frequency        | Notes                                    |
| ------------ | ---------------- | ---------------------------------------- |
| Minor Update | Every 3–6 months | Small enhancements, non-breaking changes |
| Patch Update | As needed        | Bug fixes, security updates              |
| Major Update | 25 months        | Framework upgrades, major refactors      |

* Reserve months: June (Mid-Year Chill) & December (End-Year Freeze)

## 🧘 Sustainability Practices

* 20% creative/recovery space built into development
* Mandatory cooldowns after major launches (minimum 1 week)
* Crisis Mode Activates if:
  * Critical vulnerabilities
  * Framework-breaking issues

## 🛡️ Support

- [ ] Active Support
- [x] Limited Support (Security patches only)
- [ ] Maintenance Mode (Dependency-only updates)
- [ ] Archived (No active work planned)

## 📓 Project Notes

### 🔍 Background

A while back, I [found a tool](https://schedule.lemmings.world) to schedule articles on Lemmy. I've been posting within a few hours apart at random minutes and I wanted to something decide that for me. I had AI write the base algorithm, everything else is my own touches.

This tool was originally intended with a simple purpose: suggest some publishing times. However, when customable topics were added, it became more versatile. It can be used with anything that can be scheduled to publish, from news, art, or anything else.

## 📄 License

I hereby waive this project under the public domain - see [UNLICENSE](UNLICENSE) for details.
